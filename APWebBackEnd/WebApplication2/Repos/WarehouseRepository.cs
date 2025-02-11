using APWeb.Dtos;
using APWeb.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APWeb.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using APWeb.Service.Interface;
using System.ComponentModel;

namespace APWeb.Repos
{ 
    public class WarehouseRepository(ApplicationDbContext context, ICommonService commonService)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ICommonService _commonService = commonService;


        // Retrieves the ItemID for a given ProductID and SupplierID
        // public async Task<int?> GetItemID(string ProductID, int SupplierID)
        // {
        //     var item = await _context.ProductItems.FirstOrDefaultAsync(p => p.ProductID == ProductID && p.SupplierID == SupplierID);

        //     if (item == null) return null;
        //     return item.ItemID;
        // }

        // // Retrieves the SupplierID for a given ItemID
        // private async Task<int> GetSupplierIDByItemID(int ItemID)
        // {
        //     var product = await _context.ProductItems.FindAsync(ItemID);
        //     return product.SupplierID;
        // }

        // // Retrieves the total quantity of a product in a specific store
        // public async Task<double> GetStoreProductQty(string productID, int StoreID)
        // {
        //     var totalQty = await _context.WarehouseInventory
        //                                .Where(item => item.ProductID == productID && item.StoreID == StoreID)
        //                                .SumAsync(item => item.UnitQty);

        //     return totalQty;
        // }

        // // Validates the AddInWarehouseDto to ensure the product and supplier exist
        // private async Task<(bool isValid, string errorMessage)> ValidateAddInWarehouseDto(AddInWarehouseDto adto)
        // {
        //     var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == adto.ProductID);
        //     if (product == null)
        //     {
        //         return (false, $"Product with UPC {adto.ProductID} not found.");
        //     }

        //     if (adto.UnitQty < 0)
        //     {
        //         return (false, $"UnitQty must be greater than 0.");
        //     }

        //     if (!await _context.Suppliers.AnyAsync(s => s.Supplier_ID == adto.SupplierID))
        //     {
        //         return (false, $"Supplier with SupplierID {adto.SupplierID} not found.");
        //     }

        //     return (true, string.Empty);
        // }

        // // Adds a single warehouse inventory item
        // public async Task<int> AddInWarehouse(AddInWarehouseDto adto)
        // {
        //     // Validate the DTO
        //     var validation = await ValidateAddInWarehouseDto(adto);
        //     if (!validation.isValid)
        //     {
        //         throw new Exception(validation.errorMessage);
        //     }

        //     // Get the item ID and product details
        //     var item_id = await GetItemID(adto.ProductID, adto.SupplierID);
        //     var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == adto.ProductID);

        //     // Check if the inventory already exists
        //     var inventory = await _context.WarehouseInventory
        //         .FirstOrDefaultAsync(i => i.ProductID == adto.ProductID && i.SupplierID == adto.SupplierID && i.StoreID == adto.StoreID);

        //     if (inventory == null)
        //     {
        //         // If inventory does not exist, create a new record
        //         inventory = adto.AddInWarehouseDtoToInventory(product, item_id);
        //         await _context.AddAsync(inventory);
        //     }
        //     else
        //     {
        //         // If inventory exists, update the quantity and date
        //         inventory.UnitQty += adto.UnitQty;
        //         if (inventory.Date < adto.Date)
        //         {
        //             inventory.Date = adto.Date;
        //         }
        //     }

        //     // Create a transaction record
        //     var transaction = adto.AddInWarehouseDtoToTransaction(product, item_id);
        //     await _context.AddAsync(transaction);

        //     // Save changes to the database
        //     await _context.SaveChangesAsync();

        //     return inventory.ID;
        // }

        // // Adds multiple warehouse inventory items in a batch
        // public async Task<List<int>> AddInWarehouseBatch(List<AddInWarehouseDto> addInWarehouseDtos)
        // {
        //     var inventoryIds = new List<int>();

        //     using (var dbTransaction = await _context.Database.BeginTransactionAsync())
        //     {
        //         try
        //         {
        //             foreach (var adto in addInWarehouseDtos)
        //             {
        //                 // Validate each DTO
        //                 var validation = await ValidateAddInWarehouseDto(adto);
        //                 if (!validation.isValid)
        //                 {
        //                     throw new Exception(validation.errorMessage);
        //                 }

        //                 // Get the item ID and product details
        //                 var item_id = await GetItemID(adto.ProductID, adto.SupplierID);
        //                 var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == adto.ProductID);

        //                 // Check if the inventory already exists
        //                 var inventory = await _context.WarehouseInventory
        //                     .FirstOrDefaultAsync(i => i.ProductID == adto.ProductID && i.SupplierID == adto.SupplierID && i.StoreID == adto.StoreID);

        //                 if (inventory == null)
        //                 {
        //                     // If inventory does not exist, create a new record
        //                     inventory = adto.AddInWarehouseDtoToInventory(product, item_id);
        //                     inventory.UnitQty = adto.UnitQty;
        //                     await _context.AddAsync(inventory);
        //                 }
        //                 else
        //                 {
        //                     // If inventory exists, update the quantity and date
        //                     inventory.UnitQty += adto.UnitQty;
        //                     if (inventory.Date < adto.Date)
        //                     {
        //                         inventory.Date = adto.Date;
        //                     }
        //                 }

        //                 // Create a transaction record
        //                 var transactionRecord = adto.AddInWarehouseDtoToTransaction(product, item_id);
        //                 await _context.AddAsync(transactionRecord);

        //                 // Save changes to the database
        //                 await _context.SaveChangesAsync();

        //                 // Add the inventory ID to the list
        //                 inventoryIds.Add(inventory.ID);
        //             }

        //             // Commit the transaction
        //             await dbTransaction.CommitAsync();
        //         }
        //         catch (Exception)
        //         {
        //             // Rollback the transaction in case of an error
        //             await dbTransaction.RollbackAsync();
        //             throw;
        //         }
        //     }

        //     return inventoryIds;
        // }

        // // Validates the TakeOutWarehouseDto to ensure the product exists and there is enough stock
        // private async Task<(bool isValid, string errorMessage)> ValidateTakeOutWarehouseDto(TakeOutWarehouseDto tdto)
        // {
        //     var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == tdto.ProductID);
        //     if (product == null)
        //     {
        //         return (false, $"Product with UPC {tdto.ProductID} not found.");
        //     }

        //     if (tdto.UnitQty < 0)
        //     {
        //         return (false, $"UnitQty must be greater than 0.");
        //     }

        //     if (await GetStoreProductQty(tdto.ProductID, tdto.Source) < tdto.UnitQty)

        //     {
        //         return (false, $"Insufficient stock for UPC {tdto.ProductID}.");
        //     }

        //     return (true, string.Empty);
        // }

        // // Takes out a single warehouse inventory item
        // public async Task<int> TakeOutWarehouse(TakeOutWarehouseDto tdto)
        // {
        //     // Validate the DTO
        //     var validation = await ValidateTakeOutWarehouseDto(tdto);
        //     if (!validation.isValid)
        //     {
        //         throw new Exception(validation.errorMessage);
        //     }

        //     var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == tdto.ProductID);

        //     // Get the list of inventories for the product
        //     var inventories = await _context.WarehouseInventory
        //         .Where(i => i.ProductID == tdto.ProductID && i.UnitQty != 0 && i.StoreID == tdto.Source)
        //         .OrderBy(i => i.Date)
        //         .ToListAsync();

        //     var current_qty = tdto.UnitQty; // Total quantity to be taken out
        //     var indexer = 0;

        //     // Loop through the inventories and take out the required quantity
        //     while (current_qty > 0 && indexer < inventories.Count)
        //     {
        //         var transaction_qty = Math.Min(inventories[indexer].UnitQty, current_qty);  // 取最小值作为提取量
        //         inventories[indexer].UnitQty -= transaction_qty;  // 减少库存
        //         current_qty -= transaction_qty;  // 减少还需要的数量

        //         // Create a transaction record
        //         var supplierID = inventories[indexer].SupplierID;
        //         var itemID = await GetItemID(tdto.ProductID, supplierID);
        //         await _context.AddAsync(tdto.TakeOutWarehouseDtoToTransaction(product, transaction_qty, itemID, supplierID));

        //         indexer++;  // Move to the next inventory item
        //     }

        //     // Save changes to the database
        //     await _context.SaveChangesAsync();
        //     return 0;  // 成功提取完毕
        // }

        // // Takes out multiple warehouse inventory items in a batch
        // public async Task<int> TakeOutWarehouseBatch(List<TakeOutWarehouseDto> tdtoList)
        // {
        //     using (var dbtransaction = await _context.Database.BeginTransactionAsync())  // Start a database transaction
        //     {
        //         try
        //         {
        //             foreach (var tdto in tdtoList)
        //             {
        //                 // Validate each DTO
        //                 var validation = await ValidateTakeOutWarehouseDto(tdto);
        //                 if (!validation.isValid)
        //                 {
        //                     throw new Exception(validation.errorMessage);
        //                 }

        //                 var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == tdto.ProductID);

        //                 // Get the list of inventories for the product
        //                 var inventories = await _context.WarehouseInventory
        //                     .Where(i => i.ProductID == tdto.ProductID && i.UnitQty != 0 && i.StoreID == tdto.Source)
        //                     .ToListAsync();

        //                 var current_qty = tdto.UnitQty;
        //                 var indexer = 0;

        //                 // Loop through the inventories and take out the required quantity
        //                 while (current_qty > 0 && indexer < inventories.Count)
        //                 {
        //                     var transaction_qty = Math.Min(inventories[indexer].UnitQty, current_qty);
        //                     inventories[indexer].UnitQty -= transaction_qty;  // Reduce the inventory quantity
        //                     current_qty -= transaction_qty;

        //                     // Create a transaction record
        //                     var supplierID = inventories[indexer].SupplierID;
        //                     var itemID = await GetItemID(tdto.ProductID, supplierID);
        //                     await _context.AddAsync(tdto.TakeOutWarehouseDtoToTransaction(product, transaction_qty, itemID, supplierID));

        //                     indexer++;  // Move to the next inventory item
        //                 }
        //             }

        //             // Save changes to the database
        //             await _context.SaveChangesAsync();
        //             await dbtransaction.CommitAsync();  // Commit the transaction
        //         }
        //         catch (Exception)
        //         {
        //             await dbtransaction.RollbackAsync();  // Rollback the transaction in case of an error
        //             throw;  // Rethrow the exception
        //         }
        //     }

        //     return 0;  // Successfully taken out in batch
        // }


        // public async Task<int> UndoTransaction(int transactionID)
        // {
        //     // Find the transaction by its ID
        //     var transaction = await _context.WarehouseTransactions.FindAsync(transactionID)
        //         ?? throw new Exception($"Transaction with ID {transactionID} not found.");

        //     // Check if the transaction is already an 'Undo' transaction
        //     if (transaction.Action.StartsWith("Undo"))
        //     {
        //         throw new Exception($"Cannot undo an 'Undo' transaction.");
        //     }

        //     // Check if the transaction is already undone
        //     if (transaction.Action.EndsWith("(Undone)"))
        //     {
        //         throw new Exception($"Transaction with ID {transactionID} is already undone.");
        //     }

        //     // Find the inventory associated with the transaction
        //     var inventory = await _context.WarehouseInventory.FirstOrDefaultAsync(i => i.StoreID == transaction.StoreID && i.ProductID == transaction.ProductID && i.SupplierID == transaction.SupplierID)
        //         ?? throw new Exception($"Inventory with ID {transaction.ItemID} not found.");

        //     // If the transaction is an 'Add In' action, reduce the inventory quantity
        //     if (transaction.Action == "Add In")
        //     {
        //         if (inventory.UnitQty < transaction.UnitQty)
        //         {
        //             throw new Exception($"Inventory with ID {transaction.ItemID} has insufficient stock.");
        //         }
        //         inventory.UnitQty -= transaction.UnitQty;
        //     }

        //     // If the transaction is a 'Take Out' action, increase the inventory quantity
        //     if (transaction.Action == "Take Out")
        //     {
        //         inventory.UnitQty += transaction.UnitQty;
        //     }

        //     // Create a new undo transaction record
        //     var undo_transaction = new WarehouseTransaction
        //     {
        //         ProductID = transaction.ProductID,
        //         UnitQty = transaction.UnitQty,
        //         Date = transaction.Date,
        //         Action = $"Undo {transaction.Action} {transaction.TransactionID}",
        //         SupplierID = transaction.SupplierID,
        //         ItemID = transaction.ItemID,
        //         StoreID = transaction.StoreID,
        //         POID = transaction.POID,
        //         ProductName = transaction.ProductName,
        //     };

        //     // Mark the original transaction as undone
        //     transaction.Action = $"{transaction.Action} (Undone)";

        //     // Add the undo transaction to the context and save changes
        //     await _context.AddAsync(undo_transaction);
        //     await _context.SaveChangesAsync();
        //     return undo_transaction.TransactionID;
        // }

        // public async Task<List<int>> UndoWarehouseTransactionByDto(UndoWarehouseTransactionDto dto)
        // {
        //     using (var dbtransaction = await _context.Database.BeginTransactionAsync())
        //     {
        //         var transactionList = await _context.WarehouseTransactions
        //             .Where(t =>
        //                 t.Date.Date == dto.Date
        //                 && t.ProductID == dto.ProductID
        //                 && t.Action == dto.Action
        //                 && t.StoreID == dto.StoreID
        //                 && t.POID == dto.POID
        //                 && t.SellTo == dto.SellTo)
        //             .ToListAsync();
        //         if (transactionList.Count == 0)
        //         {
        //             throw new Exception("Transaction not found.");
        //         }
        //         try
        //         {
        //             var undoTransactionIDs = new List<int>();
        //             foreach (var transaction in transactionList)
        //             {
        //                 var undoTransactionID = await UndoTransaction(transaction.TransactionID);
        //                 undoTransactionIDs.Add(undoTransactionID);
        //             }
        //             dbtransaction.Commit();
        //             return undoTransactionIDs;
        //         }
        //         catch
        //         {
        //             dbtransaction.Rollback();
        //             throw;
        //         }
        //     }
        // }


        // public async Task<int> AdjustWarehouseInventory(AdjustWarehouseInventoryDto adto)
        // {
        //     // Validate the DTO
        //     var validation = await ValidateAdjustWarehouseInventoryDto(adto);
        //     if (!validation.isValid)
        //     {
        //         throw new Exception(validation.errorMessage);
        //     }

        //     int? item_id = null;
        //     WarehouseInventory inventory;
        //     int supplierID;

        //     if (adto.SupplierID == null)
        //     {
        //         inventory = await _context.WarehouseInventory
        //             .FirstOrDefaultAsync(i => i.ProductID == adto.ProductID && i.StoreID == adto.StoreID);
        //         item_id = await GetItemID(adto.ProductID, inventory.SupplierID);
        //         supplierID = inventory.SupplierID;
        //     }
        //     else
        //     {
        //         inventory = await _context.WarehouseInventory
        //             .FirstOrDefaultAsync(i => i.ProductID == adto.ProductID && i.SupplierID == adto.SupplierID && i.StoreID == adto.StoreID);
        //         item_id = await GetItemID(adto.ProductID, (int)adto.SupplierID);
        //         supplierID = (int)adto.SupplierID;
        //     }

        //     var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == adto.ProductID);

        //     // Check if the inventory exists
        //     if (inventory == null)
        //     {
        //         throw new Exception($"Inventory record for ProductID {adto.ProductID} and SupplierID {adto.SupplierID} at StoreID {adto.StoreID} not found.");
        //     }

        //     // Adjust the quantity based on the adjustment value (can be positive or negative)
        //     inventory.UnitQty += adto.UnitQty;

        //     // Create a transaction record for adjustment
        //     var transaction = adto.AdjustWarehouseDtoToTransaction(product, item_id, supplierID);
        //     await _context.AddAsync(transaction);

        //     // Save changes to the database
        //     await _context.SaveChangesAsync();

        //     return inventory.ID;
        // }

        // private async Task<(bool isValid, string errorMessage)> ValidateAdjustWarehouseInventoryDto(AdjustWarehouseInventoryDto adto)
        // {
        //     var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == adto.ProductID);
        //     if (product == null)
        //     {
        //         return (false, $"Product with UPC {adto.ProductID} not found.");
        //     }

        //     if (adto.SupplierID != null && !await _context.Suppliers.AnyAsync(s => s.Supplier_ID == adto.SupplierID))
        //     {
        //         return (false, $"Supplier with SupplierID {adto.SupplierID} not found.");
        //     }

        //     if (adto.UnitQty == 0)
        //     {
        //         return (false, $"AdjustmentQty cannot be 0.");
        //     }

        //     return (true, string.Empty);
        // }

        public bool IsValidTaxRate(decimal taxRate)
        {
            if (taxRate != 0 && taxRate != (decimal)0.13 && taxRate != (decimal)0.05)
            {
                return false;
            }

            return true;
        }

        public async Task<(bool isValid, string errorMessage)> IsDraftOrderItemDtoValid (DraftOrderItemDto draft)
        {
            if (draft == null) return (false, "Null draft object.");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == draft.ProductID);
            if (product == null)
            {
                return (false, $"Product with UPC {draft.ProductID} not found.");
            }

            if (!await _context.Suppliers.AnyAsync(s => s.Supplier_ID == draft.SupplierID))
            {
                return (false, $"Supplier with SupplierID {draft.SupplierID} not found.");
            }

            if (!await _context.ProductItems.AnyAsync(i => i.SupplierID == draft.SupplierID && i.ProductID == i.ProductID))
            {
                return (false, $"Item with ProductID {draft.ProductID} and SupplierID {draft.SupplierID} not found.");
            }

            if (draft.UnitQty <= 0)
            {
                return (false, $"AdjustmentQty cannot less than 0.");
            }

            if (draft.UnitCost <= 0)
            {
                return (false, $"AdjustmentCost cannot less than 0.");
            }

            if (draft.UnitsPerPackage <= 0)
            {
                return (false, $"AdjustmentUnitsPerPackage cannot less than 0.");
            }

            if (!IsValidTaxRate(draft.TaxRate))
            {
                return (false, $"AdjustTaxRate is not valid.");
            }

            return (true, string.Empty);
        }


        public async Task<string> GetNewOrderID (DraftOrderItemDto draft)
        {
            var storeId = draft.StoreID.ToString("D2");
            var yearID = DateTime.Now.ToString("yy");
            

            var result = await _commonService.GetServerNodeIDAsync();

            if (!result.IsSuccess)
            {
                return null;
            }

            var serverID = result.Data.ToString("D2");

            var lastOrder = await _context.Orders
                .OrderByDescending(o => o.OrderID)
                .FirstOrDefaultAsync(o => o.OrderID.Length == 11 && o.OrderID.StartsWith(storeId + yearID + serverID));
            var sequence = 0;
            if (lastOrder != null)
            {
                sequence = int.Parse(lastOrder.OrderID.Substring(6, 5));
            }
            var sequenceStr = (sequence + 1).ToString("D5");
            return storeId + yearID + serverID + sequenceStr;
        }


        public async Task<DateTime?> GetLastOrderDate(string productID, int storeID)
        {
            var lastOrder = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.ProductItem)
                .OrderByDescending(oi => oi.Order.DraftDate)
                .FirstOrDefaultAsync(oi => oi.ProductItem.ProductID == productID && oi.Order.StoreID == storeID);

            if (lastOrder == null)
            {
                return null;
            }
            return lastOrder.Order.DraftDate;
        }


        public int GetPurchaseOrderSourceWarehouseAreaID()
        {
            return 100001;
        }


        public async Task<int> GetPurchaseOrderSourceWarehouseLocationID()
        {
            var warehouseLocation = await _context.WarehouseStorageLocations.FirstOrDefaultAsync(wl => wl.WarehouseStorageAreaID == GetPurchaseOrderSourceWarehouseAreaID());
            return warehouseLocation.WarehouseLocationID;
        }


        public async Task<(bool IsSuccess, string ErrorMessage, List<OrderItem> OrderItems)> ProcessDraftOrderItemBatch(List<DraftOrderItemDto> drafts)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var processedOrderItems = new List<OrderItem>();

                foreach (var draft in drafts)
                {
                    // 重用之前的单个处理逻辑
                    var singleResult = await ProcessDraftOrderItemInternal(draft);
                    if (!singleResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return (false, singleResult.ErrorMessage, null);
                    }

                    processedOrderItems.Add(singleResult.OrderItem);
                }

                await transaction.CommitAsync();
                return (true, null, processedOrderItems);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }


        public async Task<(bool IsSuccess, string ErrorMessage, OrderItem OrderItem)> ProcessDraftOrderItemInternal(DraftOrderItemDto draft)
        {
            // 与之前的单个处理逻辑相同，但不处理事务
            try
            {
                // 查找或创建 Order
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.SupplierID == draft.SupplierID && o.OrderStatusID == 1 && o.StoreID == draft.StoreID);

                if (order == null)
                {
                    var desWarehouse = await _context.Warehouses.FirstOrDefaultAsync(wh => wh.StoreID == draft.StoreID);
                    if (desWarehouse == null)
                    {
                        return (false, "Destination Warehouse does not exist.", null);
                    }

                    var desWarehouseArea = await _context.WarehouseStorageAreas
                        .OrderBy(wha => wha.WarehouseStorageAreaID)
                        .FirstOrDefaultAsync(wha => wha.WarehouseID == desWarehouse.WarehouseID);

                    if (desWarehouseArea == null)
                    {
                        return (false, "Destination Warehouse Area does not exist.", null);
                    }

                    var newOrderID = await GetNewOrderID(draft);
                    if (newOrderID == null)
                    {
                        return (false, "Cannot get next OrderID.", null);
                    }

                    order = new Order
                    {
                        OrderID = newOrderID,
                        DraftDate = DateTime.Now,
                        OrderType = 2,
                        SourceStorageAreaID = GetPurchaseOrderSourceWarehouseAreaID(),
                        DestinationStorageAreaID = desWarehouseArea.WarehouseStorageAreaID,
                        SupplierID = draft.SupplierID,
                        StoreID = draft.StoreID,
                        OrderStatusID = 1
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                }

                var orderID = order.OrderID;

                // 获取商品 ItemID
                var item = await _context.ProductItems
                    .FirstOrDefaultAsync(pi => pi.SupplierID == draft.SupplierID && pi.ProductID == draft.ProductID);

                if (item == null)
                {
                    return (false, "Product item does not exist.", null);
                }

                var itemID = item.ItemID;

                // 获取或创建 OrderItem
                var orderItem = await _context.OrderItems
                    .FirstOrDefaultAsync(oi => oi.OrderID == orderID && oi.ProductItemID == itemID);

                if (orderItem == null)
                {
                    var desWarehouseLocation = _context.WarehouseStorageLocations
                        .OrderBy(whl => whl.WarehouseLocationID)
                        .FirstOrDefault(whl => whl.WarehouseStorageAreaID == order.DestinationStorageAreaID);

                    if (desWarehouseLocation == null)
                    {
                        return (false, "Destination Warehouse Location does not exist.", null);
                    }

                    orderItem = new OrderItem
                    {
                        OrderID = orderID,
                        ProductItemID = itemID,
                        SourceWarehouseLocationID = await GetPurchaseOrderSourceWarehouseLocationID(),
                        DestinationWarehouseLocationID = desWarehouseLocation.WarehouseLocationID,
                        UnitQty = draft.UnitQty,
                        UnitCost = draft.UnitCost,
                        UnitsPerPackage = draft.UnitsPerPackage,
                        TaxRate = draft.TaxRate,
                        DraftDate = DateTime.Now,
                        OrderItemStatusID = 1

                    };

                    _context.OrderItems.Add(orderItem);
                }
                else
                {
                    // 更新已有的 OrderItem
                    orderItem.UnitQty = draft.UnitQty;
                    orderItem.UnitCost = draft.UnitCost;
                    orderItem.UnitsPerPackage = draft.UnitsPerPackage;
                    orderItem.TaxRate = draft.TaxRate;
                }

                await _context.SaveChangesAsync();
                return (true, null, orderItem);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

    }
}
