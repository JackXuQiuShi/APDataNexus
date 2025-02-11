
using APWeb.Models;
using APWeb.Service.Interface;
using APWeb.Service.Models;
using APWeb.Dtos;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc;



namespace APWeb.Service.Implementation
{
    public class ProductMovementService : IProductMovementService
    {
        private readonly ApplicationDbContext _context;

        public ProductMovementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<int>> DraftProductMovementAsync(string orderID)
        {
            // Retrieve the order and include related entities
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.ProductMovements)
                .FirstOrDefaultAsync(o => o.OrderID == orderID);

            if (order == null)
            {
                return ServiceResult<int>.FailureResult($"Order with ID {orderID} not found.");
            }

            switch (order.OrderStatusID)
            {
                case 1:
                    return ServiceResult<int>.FailureResult($"Order with ID {orderID} needs to be submitted first.");
                case 2:
                    break;
                case 3:
                    return ServiceResult<int>.FailureResult($"Order with ID {orderID} has already been completed.");
                default:
                    return ServiceResult<int>.FailureResult($"Order with ID {orderID} with UNDEFINED status {order.OrderStatusID}.");
            }

            // Check if there's an existing ProductMovement for this order
            var existingMovements = order.ProductMovements;
            var draftMovement = existingMovements.FirstOrDefault(pm => pm.MovementStatusID == 1); // Draft

            // If no draft movement exists, create a new ProductMovement
            if (draftMovement == null)
            {
                draftMovement = new ProductMovement
                {
                    OrderID = orderID,
                    MovementID = existingMovements.Any() ? existingMovements.Max(pm => pm.MovementID) + 1 : 1,
                    MovementType = order.OrderType, // Assuming MovementType aligns with OrderType
                    SourceStorageAreaID = order.SourceStorageAreaID,
                    DestinationStorageAreaID = order.DestinationStorageAreaID,
                    DraftDate = DateTime.Now,
                    MovementStatusID = 1, // Draft
                    Quantity = 0,
                    TotalCost = 0
                };

                _context.ProductMovements.Add(draftMovement);
                await _context.SaveChangesAsync();
            }

            try
            { 
                // Iterate through OrderItems and create/update ProductMovementItems
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem.OrderItemStatusID < 3)
                    {
                        var movementItem = draftMovement.ProductMovementItems
                            .FirstOrDefault(pmi => pmi.ProductItemID == orderItem.ProductItemID);

                        if (movementItem == null)
                        {
                            // Create a new ProductMovementItem
                            movementItem = new ProductMovementItem
                            {
                                OrderID = orderID,
                                MovementID = draftMovement.MovementID,
                                ProductItemID = orderItem.ProductItemID,
                                SourceWarehouseLocationID = orderItem.SourceWarehouseLocationID,
                                DestinationWarehouseLocationID = orderItem.DestinationWarehouseLocationID,
                                UnitQty = orderItem.UnitQty,
                                UnitCost = orderItem.UnitCost,
                                QuantityRemaining = orderItem.UnitQty,
                                ItemStatusID = 1, // Draft
                                DraftDate = DateTime.Now
                            };

                            _context.ProductMovementItems.Add(movementItem);
                        }
                        else
                        {
                            // Update existing ProductMovementItem
                            if (movementItem.ItemStatusID < 2)
                            {
                                movementItem.UnitQty = orderItem.UnitQty;
                                movementItem.UnitCost = orderItem.UnitCost;
                                movementItem.QuantityRemaining = orderItem.UnitQty;
                                movementItem.DraftDate = DateTime.Now;

                                _context.ProductMovementItems.Update(movementItem);
                            }
                        }
                    }
                }

                // Save changes to ProductMovementItems
                await _context.SaveChangesAsync();

                // Update the draft ProductMovement totals
                draftMovement.Quantity = draftMovement.ProductMovementItems.Sum(pmi => pmi.UnitQty);
                draftMovement.TotalCost = draftMovement.ProductMovementItems.Sum(pmi => pmi.UnitQty * pmi.UnitCost);

                _context.ProductMovements.Update(draftMovement);

                // Persist changes to ProductMovement
                await _context.SaveChangesAsync();

                return ServiceResult<int>.SuccessResult(draftMovement.MovementID, "ProductMovement and ProductMovementItems drafted successfully.");

            }
            catch (Exception ex)
            {
                return ServiceResult<int>.FailureResult($"Error during submission: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> UpdateProductMovementItemAsync(ProductMovementItemRequest pmiRequest)
        {
            // Validate the ProductMovement
            var productMovement = await _context.ProductMovements
                .Include(pm => pm.ProductMovementItems)
                .FirstOrDefaultAsync(pm => pm.OrderID == pmiRequest.OrderID && pm.MovementID == pmiRequest.MovementID);

            if (productMovement == null)
            {
                return ServiceResult<bool>.FailureResult($"ProductMovement with OrderID {pmiRequest.OrderID} and " +
                                                             $"ProductMovementID {pmiRequest.MovementID} not found.");
            }

            switch (productMovement.MovementStatusID)
            {
                case 1:
                    break;
                case 2:
                    return ServiceResult<bool>.FailureResult($"ProductMovement with OrderID {pmiRequest.OrderID} and " +
                                                        $"ProductMovementID {pmiRequest.MovementID} is already submitted.");
                default:
                    return ServiceResult<bool>.FailureResult($"ProductMovement with OrderID {pmiRequest.OrderID} and " +
                                                        $"ProductMovementID {pmiRequest.MovementID} is with unknown status.");
            }

            switch (pmiRequest.ProductMovementItemStatusID)
            {
                case 1:
                    break;
                case 2:
                    break;
                default:
                    return ServiceResult<bool>.FailureResult($"The input ProductMovement StatusID {pmiRequest.ProductMovementItemStatusID} is invalid.");
            }

            try 
            { 
                var MovementItem = await _context.ProductMovementItems
                    .FirstOrDefaultAsync(pmi => pmi.OrderID == pmiRequest.OrderID
                                                && pmi.MovementID == pmiRequest.MovementID
                                                && pmi.ProductItemID == pmiRequest.ProductItemID);

                if (MovementItem != null)
                {
                    MovementItem.UnitQty = pmiRequest.UnitQty;
                    MovementItem.UnitCost = pmiRequest.UnitCost;
                    MovementItem.QuantityRemaining = pmiRequest.UnitQty;

                    switch (MovementItem.ItemStatusID)
                    {
                        case 1:
                            MovementItem.DraftDate = DateTime.Now;
                            break;
                        case 2:
                            return ServiceResult<bool>.FailureResult($"ProductMovementItem with OrderID {pmiRequest.OrderID} " +
                                $"MovementID {pmiRequest.MovementID} and ProductItemID {pmiRequest.ProductItemID} is " +
                                $"already submitted.");

                        default:
                            return ServiceResult<bool>.FailureResult($"ProductMovementItem with OrderID {pmiRequest.OrderID} " +
                                $"MovementID {pmiRequest.MovementID} and ProductItemID {pmiRequest.ProductItemID} is " +
                                $"with UNDEFINED status {MovementItem.ItemStatusID}.");

                    }
                    if (pmiRequest.ProductMovementItemStatusID == 2)
                    {
                        MovementItem.SubmitDate = DateTime.Now;
                    }
                    MovementItem.ItemStatusID = pmiRequest.ProductMovementItemStatusID;
                    _context.ProductMovementItems.Update(MovementItem);
                }
                else
                {
                    MovementItem = new ProductMovementItem
                    {
                        OrderID = pmiRequest.OrderID,
                        MovementID = pmiRequest.MovementID,
                        ProductItemID = pmiRequest.ProductItemID,
                        SourceWarehouseLocationID = pmiRequest.SourceWarehouseLocationID,
                        DestinationWarehouseLocationID = pmiRequest.DestinationWarehouseLocationID,
                        UnitQty = pmiRequest.UnitQty,
                        UnitCost = pmiRequest.UnitCost,
                        QuantityRemaining = pmiRequest.UnitQty,
                        ItemStatusID = pmiRequest.ProductMovementItemStatusID
                    };
                    switch (pmiRequest.ProductMovementItemStatusID)
                    {
                        case 1:
                            MovementItem.DraftDate = DateTime.Now;
                            break;
                        case 2:
                            MovementItem.DraftDate = DateTime.Now;
                            MovementItem.SubmitDate = DateTime.Now;
                            break;
                        default:
                            return ServiceResult<bool>.FailureResult($"ProductMovementItem with OrderID {pmiRequest.OrderID} " +
                                $"MovementID {pmiRequest.MovementID} and ProductItemID {pmiRequest.ProductItemID} is " +
                                $"with UNDEFINED status {pmiRequest.ProductMovementItemStatusID}.");

                    }
                    _context.ProductMovementItems.Add(MovementItem);
                }

                await _context.SaveChangesAsync();

                // Recalculate ProductMovement totals after persisting changes
                var allMovementItems = await _context.ProductMovementItems
                .Where(pmi => pmi.OrderID == pmiRequest.OrderID && pmi.MovementID == pmiRequest.MovementID)
                .ToListAsync();

                productMovement.Quantity = allMovementItems.Sum(pmi => pmi.UnitQty);
                productMovement.TotalCost = allMovementItems.Sum(pmi => pmi.UnitQty * pmi.UnitCost);

                _context.ProductMovements.Update(productMovement);

                // Persist updated ProductMovement
                await _context.SaveChangesAsync();

                //var sourceLocationID = MovementItem.SourceWarehouseLocationID;
                //var destinationLocationID = MovementItem.DestinationWarehouseLocationID;

                if (MovementItem.ItemStatusID == 2)
                {   // Adjust inventory at source and destination
                    var inventoryResult = await AdjustInventoryFIFOAsync(
                        MovementItem.ProductItemID,
                        MovementItem.SourceWarehouseLocationID,
                        MovementItem.DestinationWarehouseLocationID,
                        MovementItem.UnitQty,
                        MovementItem.MovementID,
                        productMovement.MovementType,
                        MovementItem.UnitCost
                    );

                    if (!inventoryResult.IsSuccess)
                    {
                        return ServiceResult<bool>.FailureResult(inventoryResult.ErrorMessage);
                    }

                    if (productMovement.MovementType == 1)
                    {
                        MovementItem.UnitCost = inventoryResult.Data;
                        _context.ProductMovementItems.Update(MovementItem);
                        await _context.SaveChangesAsync();

                        productMovement.TotalCost = allMovementItems.Sum(pmi => pmi.UnitQty * pmi.UnitCost);
                        _context.ProductMovements.Update(productMovement);
                        // Persist updated ProductMovement
                        await _context.SaveChangesAsync();

                    }

                    var orderItem = await _context.OrderItems
                    .FirstOrDefaultAsync(oi => oi.OrderID == pmiRequest.OrderID
                                               && oi.ProductItemID == pmiRequest.ProductItemID);
                    orderItem.OrderItemStatusID = 3;
                    orderItem.CompleteDate = DateTime.Now;
                    _context.OrderItems.Update(orderItem);
                    await _context.SaveChangesAsync();

                    // Update corresponding OrderItems
                    var orderItems = await _context.OrderItems
                        .Where(oi => oi.OrderID == pmiRequest.OrderID)
                        .ToListAsync();

                    
                    // Check if all OrderItems are COMPLETE and update the Order status
                    if (orderItems.All(oi => oi.OrderItemStatusID == 3))
                    {
                        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == pmiRequest.OrderID);
                        if (order != null)
                        {
                            order.OrderStatusID = 3; // COMPLETE
                            order.CompleteDate = DateTime.Now;
                            _context.Orders.Update(order);
                        }
                    }

                    await _context.SaveChangesAsync();

                }

                return ServiceResult<bool>.SuccessResult(true, "ProductMovement item processed, and inventory updated successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.FailureResult($"Error during submission: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> SubmitProductMovementAsync(ProductMovementRequest pmRequest)
        {
            // Validate the ProductMovement
            var productMovement = await _context.ProductMovements
                .Include(pm => pm.ProductMovementItems)
                .FirstOrDefaultAsync(pm => pm.OrderID == pmRequest.OrderID && pm.MovementID == pmRequest.ProductMovementID);

            if (productMovement == null)
            {
                return ServiceResult<bool>.FailureResult($"ProductMovement with OrderID {pmRequest.OrderID} and " +
                                                             $"ProductMovementID {pmRequest.ProductMovementID} not found.");
            }

            switch (productMovement.MovementStatusID)
            {
                case 1:
                    break;
                case 2:
                    return ServiceResult<bool>.FailureResult($"ProductMovement with OrderID {pmRequest.OrderID} and " +
                                                        $"ProductMovementID {pmRequest.ProductMovementID} is already submitted.");
                default:
                    return ServiceResult<bool>.FailureResult($"ProductMovement with OrderID {pmRequest.OrderID} and " +
                                                        $"ProductMovementID {pmRequest.ProductMovementID} is with unknown status.");
            }

            
            // Perform the submission logic and update statuses
            try
            {
                foreach (var item in productMovement.ProductMovementItems)
                {
                    if (item.ItemStatusID >= 2)
                    {
                        continue;
                    }

                    // Update inventory and item status
                    var inventoryResult =  await AdjustInventoryFIFOAsync(item.ProductItemID, item.SourceWarehouseLocationID,
                                                   item.DestinationWarehouseLocationID, item.UnitQty,
                                                   productMovement.MovementID, productMovement.MovementType, item.UnitCost);

                    
                    if (!inventoryResult.IsSuccess)
                    {
                        return ServiceResult<bool>.FailureResult(inventoryResult.ErrorMessage);
                    }

                    if (productMovement.MovementType == 1)
                    {
                        item.UnitCost = inventoryResult.Data;
                    }
                    item.ItemStatusID = 2;
                    item.SubmitDate = DateTime.Now;
                    _context.ProductMovementItems.Update(item);

                }

                // Recalculate ProductMovement totals after persisting changes
                var allMovementItems = await _context.ProductMovementItems
                    .Where(pmi => pmi.OrderID == pmRequest.OrderID && pmi.MovementID == pmRequest.ProductMovementID)
                    .ToListAsync();

                productMovement.TotalCost = allMovementItems.Sum(pmi => pmi.UnitQty * pmi.UnitCost);
                productMovement.MovementStatusID = 2;
                productMovement.SubmitDate = DateTime.Now;
                _context.ProductMovements.Update(productMovement);
                await _context.SaveChangesAsync();

                // Update corresponding OrderItems
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderID == pmRequest.OrderID)
                    .ToListAsync();

                foreach (var orderItem in orderItems)
                {
                    if (productMovement.ProductMovementItems.Any(pmi => pmi.ProductItemID == orderItem.ProductItemID &&
                                                                        pmi.ItemStatusID == 2))
                    {
                        orderItem.OrderItemStatusID = 3; // COMPLETE
                        orderItem.CompleteDate = DateTime.Now;
                        _context.OrderItems.Update(orderItem);
                    }
                }

                // Check if all OrderItems are COMPLETE and update the Order status
                if (orderItems.All(oi => oi.OrderItemStatusID == 3))
                {
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == pmRequest.OrderID);
                    if (order != null)
                    {
                        order.OrderStatusID = 3; // COMPLETE
                        order.CompleteDate = DateTime.Now;
                        _context.Orders.Update(order);
                    }
                }

                await _context.SaveChangesAsync();

                return ServiceResult<bool>.SuccessResult(true, "ProductMovement submitted successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.FailureResult($"Error during submission: {ex.Message}");
            }
        }

        private async Task<ServiceResult<bool>> MoveOrderItemAsync(string orderID, int productItemID, decimal quantity, decimal unitCost)
        {
            var order = await _context.Orders
                .Include(o => o.ProductMovements)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderID == orderID);

            if (order == null)
            {
                return ServiceResult<bool>.FailureResult($"Order with ID {orderID} not found.");
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderID == orderID && oi.ProductItemID == productItemID);

            if (orderItem == null)
            {
                return ServiceResult<bool>.FailureResult($"Order item with ID {productItemID} for order {orderID} not found.");
            }

            if (order.OrderType == 1)
            // Fetch or create Inventory record
            {
                var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ItemID == productItemID && i.LocationID == orderItem.SourceWarehouseLocationID);

                if (inventory != null && inventory.CurrentStock < quantity)
                {
                    return ServiceResult<bool>.FailureResult($"Insufficient inventory to fulfill the shipping request for ProductItemID {productItemID}.");
                }

            }
            var movementType = order.OrderType;

            // Check for an existing ProductMovement with status "Draft"
            var productMovement = order.ProductMovements
                .OrderByDescending(pm => pm.MovementID)
                .FirstOrDefault(pm => pm.MovementStatusID == 0);

            if (productMovement == null)
            {
                productMovement = new ProductMovement
                {
                    OrderID = orderID,
                    MovementID = order.ProductMovements.Any() ? order.ProductMovements.Max(pm => pm.MovementID) + 1 : 1,
                    MovementType = movementType, // Set the movement type (1: Shipping, 2: Receiving)
                    SourceStorageAreaID = order.SourceStorageAreaID,
                    DestinationStorageAreaID = order.DestinationStorageAreaID,
                    DraftDate = DateTime.Now,
                    Quantity = 0,
                    TotalCost = 0,
                    MovementStatusID = 0
                };
                _context.ProductMovements.Add(productMovement);
                await _context.SaveChangesAsync();
            }

            var movementID = productMovement.MovementID;

            // Check or create ProductMovementItem
            var MovementItem = await _context.ProductMovementItems
                .FirstOrDefaultAsync(pmi => pmi.OrderID == orderID
                                            && pmi.MovementID == productMovement.MovementID
                                            && pmi.ProductItemID == productItemID);

            if (MovementItem != null)
            {
                MovementItem.UnitQty = quantity;
                MovementItem.UnitCost = unitCost;
                MovementItem.QuantityRemaining = quantity;
                _context.ProductMovementItems.Update(MovementItem);
            }
            else
            {
                MovementItem = new ProductMovementItem
                {
                    OrderID = orderID,
                    MovementID = productMovement.MovementID,
                    ProductItemID = productItemID,
                    SourceWarehouseLocationID = orderItem.SourceWarehouseLocationID,
                    DestinationWarehouseLocationID = orderItem.DestinationWarehouseLocationID,
                    UnitQty = quantity,
                    UnitCost = unitCost,
                    QuantityRemaining = quantity,
                    ItemStatusID = 0
                };
                _context.ProductMovementItems.Add(MovementItem);
            }

            await _context.SaveChangesAsync();

            // Recalculate ProductMovement totals after persisting changes
            var allMovementItems = await _context.ProductMovementItems
                .Where(pmi => pmi.OrderID == orderID && pmi.MovementID == productMovement.MovementID)
                .ToListAsync();

            productMovement.Quantity = allMovementItems.Sum(pmi => pmi.UnitQty);
            productMovement.TotalCost = allMovementItems.Sum(pmi => pmi.UnitQty * pmi.UnitCost);

            _context.ProductMovements.Update(productMovement);

            // Persist updated ProductMovement
            await _context.SaveChangesAsync();

            //var sourceLocationID = MovementItem.SourceWarehouseLocationID;
            //var destinationLocationID = MovementItem.DestinationWarehouseLocationID;


            // Adjust inventory at source and destination
            var inventoryResult = await AdjustInventoryFIFOAsync(
                productItemID,
                MovementItem.SourceWarehouseLocationID,
                MovementItem.DestinationWarehouseLocationID,
                quantity,
                movementID,
                movementType,
                unitCost
            );

            if (!inventoryResult.IsSuccess)
            {
                return ServiceResult<bool>.FailureResult(inventoryResult.ErrorMessage);
            }

            if (movementType == 1)
            {
                MovementItem.UnitCost = inventoryResult.Data;
                _context.ProductMovementItems.Update(MovementItem);
                await _context.SaveChangesAsync();

                productMovement.TotalCost = allMovementItems.Sum(pmi => pmi.UnitQty * pmi.UnitCost);
                _context.ProductMovements.Update(productMovement);
                // Persist updated ProductMovement
                await _context.SaveChangesAsync();

            }

            //return Ok("Order item processed, and inventory updated successfully.");
            return ServiceResult<bool>.SuccessResult(true, "Order item processed, and inventory updated successfully.");
        }

        private async Task<ServiceResult<decimal>> AdjustInventoryFIFOAsync(
            int productItemID,
            int sourceLocationID,
            int destinationLocationID,
            decimal quantity,
            int movementID,
            int movementType,
            decimal unitCost)
        {
            decimal shipUnitCost = 0;
            try
            {

                // Fetch or create source inventory record
                var sourceInventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ItemID == productItemID && i.LocationID == sourceLocationID);
                
                if (sourceInventory == null)
                {
                    // Create a new source inventory record
                    int newSourceInventoryID = 1;
                    var maxSourceInventoryID = await _context.Inventories.MaxAsync(i => (int?)i.InventoryID);
                    if (maxSourceInventoryID.HasValue)
                    {
                        newSourceInventoryID = maxSourceInventoryID.Value + 1;
                    }

                    sourceInventory = new Inventory
                    {
                        InventoryID = newSourceInventoryID,
                        ItemID = productItemID,
                        LocationID = sourceLocationID,
                        CurrentStock = 0,
                        CurrentCost = 0,
                        ReorderPoint = 0,
                        SafetyStock = 0,
                        LastReplenished = DateTime.MinValue,
                        ModTime = DateTime.Now
                    };

                    _context.Inventories.Add(sourceInventory);
                    await _context.SaveChangesAsync();
                }

                // Fetch or create destination inventory record
                var destinationInventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ItemID == productItemID && i.LocationID == destinationLocationID);

                if (destinationInventory == null)
                {
                    // Create a new destination inventory record
                    int newDestinationInventoryID = 1;
                    var maxDestinationInventoryID = await _context.Inventories.MaxAsync(i => (int?)i.InventoryID);
                    if (maxDestinationInventoryID.HasValue)
                    {
                        newDestinationInventoryID = maxDestinationInventoryID.Value + 1;
                    }

                    destinationInventory = new Inventory
                    {
                        InventoryID = newDestinationInventoryID,
                        ItemID = productItemID,
                        LocationID = destinationLocationID,
                        CurrentStock = 0,
                        CurrentCost = 0,
                        ReorderPoint = 0,
                        SafetyStock = 0,
                        LastReplenished = DateTime.MinValue,
                        ModTime = DateTime.Now
                    };

                    _context.Inventories.Add(destinationInventory);
                    await _context.SaveChangesAsync();
                }

                decimal totalCost = quantity * unitCost;

                if (movementType == 1) // Shipping
                {
                    // Deduct from source inventory

                    var movementItems = await _context.ProductMovementItems
                        .Where(pmi => pmi.ProductItemID == productItemID &&
                                        pmi.DestinationWarehouseLocationID == sourceLocationID &&
                                        pmi.QuantityRemaining > 0)
                        .OrderBy(pmi => pmi.ProductMovement.SubmitDate)
                        .ToListAsync();

                    decimal remainingQuantity = quantity;
                    totalCost = 0;

                    foreach (var batch in movementItems)
                    {
                        if (remainingQuantity <= 0)
                            break;

                        var quantityToDeduct = Math.Min(batch.QuantityRemaining, remainingQuantity);

                        totalCost += quantityToDeduct * batch.UnitCost;
                        batch.QuantityRemaining -= quantityToDeduct;
                        remainingQuantity -= quantityToDeduct;

                        _context.ProductMovementItems.Update(batch);
                    }
                }

                shipUnitCost = totalCost / quantity;

                if (sourceInventory.CurrentStock == quantity)
                {
                    sourceInventory.CurrentCost = 0;
                    sourceInventory.CurrentStock = 0;
                }
                else
                {
                    sourceInventory.CurrentCost = (sourceInventory.CurrentCost * sourceInventory.CurrentStock - totalCost) /
                                                    (sourceInventory.CurrentStock - quantity);
                    sourceInventory.CurrentStock -= quantity;
                }
                //sourceInventory.LastReplenished = DateTime.Now;
                sourceInventory.ModTime = DateTime.Now;
                _context.Inventories.Update(sourceInventory);

                destinationInventory.CurrentCost = (destinationInventory.CurrentCost * destinationInventory.CurrentStock +
                                                totalCost) / (destinationInventory.CurrentStock + quantity);
                destinationInventory.CurrentStock += quantity;
                destinationInventory.LastReplenished = DateTime.Now;
                destinationInventory.ModTime = DateTime.Now;

                _context.Inventories.Update(destinationInventory);

                await _context.SaveChangesAsync();

                //return ($"{(movementType == 1 ? "Shipping" : "Receiving")} inventory updated successfully.", shipUnitCost);
                return ServiceResult<decimal>.SuccessResult(shipUnitCost, "inventory updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<decimal>.FailureResult($"Error during inventory adjustment: {ex.Message}");
            }
        }
    }
}
