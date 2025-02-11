
using APWeb.Dtos;
using APWeb.Mappers;
using APWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using APWeb.Repos;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using APWeb.Service.Interface;
using APWeb.Service.Implementation;


namespace APWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        private readonly ApplicationDbContext _context;
        private readonly WarehouseRepository _repo;
        private readonly IProductMovementService _productMovementService;

        public POController(
            IConfiguration configuration,
            ApplicationDbContext context,
            WarehouseRepository repo,
            IProductMovementService productMovementService)
        {
            _configuration = configuration;
            sqlDataSource = configuration.GetConnectionString("PRIS_ALP_test");
            _context = context;
            _repo = repo;
            _productMovementService = productMovementService;
        }

        [HttpGet("getOrderByOrderID")]
        public async Task<IActionResult> GetOrderByOrderID(string orderID)
        {
            var orderEntity = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductItem)
                        .ThenInclude(pi => pi.Product)
                //.Include(o => o.OrderItems)
                //    .ThenInclude(oi => oi.ProductMovementItems)
                .Include(o => o.SourceStorageArea)
                    .ThenInclude(wsa => wsa.Warehouse)
                .Include(o => o.DestinationStorageArea)
                    .ThenInclude(wsa => wsa.Warehouse)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(o => o.OrderID == orderID);

            if (orderEntity == null)
            {
                return NoContent();
            }

            var orderDto = new OrderDto
            {
                OrderID = orderEntity.OrderID,
                DraftDate = orderEntity.DraftDate, // Explicit conversion
                SubmitDate = orderEntity.SubmitDate,
                CompleteDate = orderEntity.CompleteDate,
                OrderType = orderEntity.OrderType, 
                SourceStorageAreaID = orderEntity.SourceStorageAreaID,
                DestinationStorageAreaID = orderEntity.DestinationStorageAreaID,
                SupplierID = orderEntity.SupplierID,
                SupplierName = orderEntity.Supplier?.CompanyName,
                StoreID = orderEntity.StoreID,
                SourceWarehouseName = orderEntity.SourceStorageArea?.Warehouse?.WarehouseName,
                DestinationWarehouseName = orderEntity.DestinationStorageArea?.Warehouse?.WarehouseName,
                OrderStatusID = orderEntity.OrderStatusID,
                OrderItems = orderEntity.OrderItems.Select(oi => 
                {
                    var sourceInventory = _context.Inventories
                        .FirstOrDefault(wi =>
                            wi.LocationID == oi.SourceWarehouseLocationID &&
                            wi.ItemID == oi.ProductItem.ItemID);

                    var destinationInventory = _context.Inventories
                        .FirstOrDefault(wi =>
                            wi.LocationID == oi.DestinationWarehouseLocationID &&
                            wi.ItemID == oi.ProductItem.ItemID);

                    var productMovementItem = _context.ProductMovementItems
                        .FirstOrDefault(pmi => 
                            pmi.OrderID == oi.OrderID && pmi.ProductItemID == oi.ProductItemID);

                    var location = _context.StorageLocation
                        .Where(l => l.ProductID == oi.ProductItem.ProductID && l.StoreID == orderEntity.StoreID)
                        .OrderByDescending(l => l.LatestDate)
                        .FirstOrDefault();

                    return new OrderItemDto
                    {
                        ProductItemID = oi.ProductItemID,
                        ProductID = oi.ProductItem?.ProductID,
                        OrderID = oi.OrderID,
                        SourceWarehouseLocationID = oi.SourceWarehouseLocationID,
                        DestinationWarehouseLocationID = oi.DestinationWarehouseLocationID,
                        UnitQty = oi.UnitQty,
                        UnitCost = oi.UnitCost,
                        ProductName = oi.ProductItem?.Product?.ProductName,
                        UnitsPerPackage = oi.UnitsPerPackage,
                        TaxRate = oi.TaxRate,
                        OrderItemStatusID = oi.OrderItemStatusID,
                        SourceCurrentStock = sourceInventory?.CurrentStock ?? 0,
                        DestinationCurrentStock = destinationInventory?.CurrentStock ?? 0,
                        Location = location?.Location,

                        MovementID = productMovementItem != null ? productMovementItem.MovementID : 0,
                        ProductMovementSourceWarehouseLocationID = productMovementItem != null ? productMovementItem.SourceWarehouseLocationID : 0,
                        ProductMovementDestinationWarehouseLocationID = productMovementItem != null ? productMovementItem.DestinationWarehouseLocationID : 0,
                        ProductMovementUnitCost = productMovementItem != null ? productMovementItem.UnitCost : 0,
                        ProductMovementQuantityRemaining = productMovementItem != null ? productMovementItem.QuantityRemaining : 0,
                        ProductMovementItemStatusID = productMovementItem != null ? productMovementItem.ItemStatusID : 0,
                        ProductMovementUnitQty = productMovementItem != null ? productMovementItem.UnitQty : 0,
                        //ProductMovementItems = oi.ProductMovementItems.Select(pmi => pmi.ProductMovementItemToProductMovementItemDto()).ToList(),
                    };
                }).ToList()
            };

            orderDto.OrderItems = orderDto.OrderItems.OrderBy(oi => oi.OrderItemStatusID).ToList();

            return Ok(orderDto);
        }


        [HttpGet("isProductItemExist")]
        public async Task<IActionResult> IsProductItemExist(string productID, int supplierID)
        {
            var isExist = _context.ProductItems
                .Any(pi => pi.SupplierID == supplierID && pi.ProductID == productID);
            return Ok(new { isExist });
        }


        [HttpGet("getSuppliersByProductID")]
        public async Task<IActionResult> GetSuppliersByProductID(string productID)
        {
            var productItems = await _context.ProductItems
                .Include(pi => pi.Supplier)
                .Where(pi => pi.ProductID == productID)
                .ToListAsync();
            var suppliers = productItems.Select(pi => new
            {
                pi.SupplierID,
                pi.Supplier.CompanyName
            }).ToList();
            return Ok(suppliers);
        }


        [HttpPost("draftOrderItem")]
        public async Task<IActionResult> DraftOrderItem(DraftOrderItemDto draft)
        {
            // 验证输入
            var validDraftResult = await _repo.IsDraftOrderItemDtoValid(draft);
            if (!validDraftResult.isValid)
            {
                return BadRequest(new { Message = validDraftResult.errorMessage });
            }

            // 调用仓储层处理单个订单逻辑
            var result = await _repo.ProcessDraftOrderItemInternal(draft);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }

            return Ok(result.OrderItem.OrderItemToOrderItemDto());
        }



        [HttpPost("draftOrderItemBatch")]
        public async Task<IActionResult> DraftOrderItemBatch(List<DraftOrderItemDto> drafts)
        {
            if (drafts == null || !drafts.Any())
            {
                return BadRequest(new { Message = "Draft list is empty." });
            }

            // 验证输入
            foreach (var draft in drafts)
            {
                var validDraftResult = await _repo.IsDraftOrderItemDtoValid(draft);
                if (!validDraftResult.isValid)
                {
                    return BadRequest(new { Message = validDraftResult.errorMessage });
                }
            }

            // 调用仓储层处理业务逻辑
            var result = await _repo.ProcessDraftOrderItemBatch(drafts);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }

            return Ok(result.OrderItems.Select(oi => oi.OrderItemToOrderItemDto()));
        }


        [HttpPost("confirmOrder")]
        public async Task<IActionResult> ConfirmOrder(string orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 验证 Order 是否存在
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderId);
                if (order == null)
                {
                    return BadRequest($"Order with order id {orderId} does not exist.");
                }

                // 验证 Order 是否包含 OrderItems
                if (!await _context.OrderItems.AnyAsync(oi => oi.OrderID == orderId))
                {
                    return BadRequest($"The order with order id {orderId} has no order items.");
                }

                if (order.OrderStatusID != 1)
                {
                    return BadRequest($"The order with order id {orderId} cannot be confirmed again.");
                }

                // 更新 OrderItems 状态
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderID == orderId)
                    .ToListAsync();

                foreach (var item in orderItems)
                {
                    item.SubmitDate = DateTime.Now;
                    item.OrderItemStatusID = 2; // 示例：2 表示已确认状态
                }

                order.OrderStatusID = 2;
                order.SubmitDate = DateTime.Now;

                await _context.SaveChangesAsync();

                // 调用 DraftProductMovementAsync
                var result = await _productMovementService.DraftProductMovementAsync(orderId);

                if (!result.IsSuccess)
                {
                    // 如果失败，抛出异常以触发回滚
                    throw new Exception($"Failed to draft ProductMovement: {result.ErrorMessage}");
                }

                // 提交事务
                await transaction.CommitAsync();

                // 返回成功响应
                return Ok(new
                {
                    Message = $"The order with order id {orderId} is confirmed and ProductMovement is drafted.",
                    OrderID = orderId,
                    ProductMovementID = result.Data
                });
            }
            catch (Exception ex)
            {
                // 回滚事务
                await transaction.RollbackAsync();

                // 返回失败响应
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("deleteOrderItem")]
        public async Task<IActionResult> DeleteOrderItem(DeleteOrderItemDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductItem)
                .FirstOrDefaultAsync(o => o.OrderID == dto.OrderID);
            if (order == null)
            {
                return BadRequest(new { message = $"Order with order id {dto.OrderID} does not exist." });

            }

            if (order.OrderStatusID != 1)
            {
                return BadRequest(new { message = $"Order with order id {dto.OrderID} is not in draft status." });
            }
            
            var item = order.OrderItems
                .Where(oi => oi.ProductItem.ProductID == dto.ProductID)
                .FirstOrDefault();
            if (item == null)
            {
                return BadRequest(new { message = $"Order with order id {dto.OrderID} does not contain Product {dto.ProductID}." });
            }
            _context.Remove(item);
            order.OrderItems.Remove(item); // order.OrderItems 的集合不会实时更新

            if (order.OrderItems.Count == 0)
            {
                _context.Remove(order);
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Order item with Product ID {item.ProductItem.ProductID} is deleted." });
                
        }


        //[HttpGet("updateInventoryByOrderItems2")]
        //public async Task<IActionResult> UpdateInventoryByOrderItems2(string OrderID)
        //{
        //    var order = await _context.Orders
        //        .Include(o => o.OrderItems)
        //        .FirstOrDefaultAsync(o => o.OrderID == OrderID);

        //    if (order == null)
        //    {
        //        return NotFound($"Order with ID {OrderID} not found.");
        //    }

        //    if (order.OrderType != 2)
        //    {
        //        return BadRequest("Order type is not Purchase Order.");
        //    }

        //    var productMovement = new ProductMovement
        //    {
        //        OrderID = order.OrderID,
        //        MovementType = order.OrderType,
        //        MovementDate = DateTime.Now,
        //        SourceStorageAreaID = order.SourceStorageAreaID,
        //        DestinationStorageAreaID = order.DestinationStorageAreaID,
        //        Quantity = order.OrderItems.Sum(oi => oi.UnitQty)
        //    };

        //    _context.ProductMovement.Add(productMovement);
        //    await _context.SaveChangesAsync();

        //    foreach (var orderItem in order.OrderItems)
        //    {
        //        var inventory = await _context.WarehouseInventory
        //            .FirstOrDefaultAsync(wi => wi.ItemID == orderItem.ProductItemID
        //                                       && wi.WarehouseLocationID == orderItem.DestinationWarehouseLocationID);

        //        if (inventory != null)
        //        {
        //            inventory.CurrentStock += orderItem.UnitQty;
        //        }
        //        else
        //        {
        //            var newInventory = new WarehouseInventory
        //            {
        //                ItemID = orderItem.ProductItemID,
        //                WarehouseLocationID = orderItem.DestinationWarehouseLocationID,
        //                CurrentStock = orderItem.UnitQty,
        //                LastReplenished = DateOnly.FromDateTime(DateTime.Now)
        //            };

        //            _context.WarehouseInventory.Add(newInventory);
        //            await _context.SaveChangesAsync();
        //        }

        //        var productMovementItem = new ProductMovementItem
        //        {
        //            OrderID = orderItem.OrderID,
        //            MovementID = productMovement.MovementID,
        //            ProductItemID = orderItem.ProductItemID,
        //            Quantity = orderItem.UnitQty,
        //            SourceWarehouseLocationID = orderItem.SourceWarehouseLocationID,
        //            DestinationWarehouseLocationID = orderItem.DestinationWarehouseLocationID,
        //            UnitCost = orderItem.UnitCost
        //        };

        //        _context.ProductMovementItems.Add(productMovementItem);
        //    }

        //    await _context.SaveChangesAsync();

        //    return Ok("Inventory updated and movements recorded successfully.");
        //}

        [HttpGet("getOrdersSuppliersByStatusID")]
        public async Task<IActionResult> GetOrdersSuppliersByStatusID(int statusID, int storeId)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderStatusID == statusID && o.StoreID == storeId)
                .Join(
                    _context.Suppliers,
                    order => order.SupplierID,
                    supplier => supplier.Supplier_ID,
                    (order, supplier) => new
                    {
                        SupplierID = supplier.Supplier_ID,
                        CompanyName = supplier.CompanyName
                    })
                .GroupBy(x => x.SupplierID) // 按 SupplierID 分组
                .Select(g => g.First()) // 取每组的第一个元素
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("getOrdersByStatusIDAndStoreID")]
        public async Task<IActionResult> GetOrdersByStatusIDAndStoreID(int statusID, int storeId, int SupplierID)
        {
            var query = _context.Orders.AsQueryable();

            if (SupplierID != 0)
            {
                query = query.Where(o => o.SupplierID == SupplierID);
            }
            var temp = await query
                .Where(o => o.OrderStatusID == statusID && o.StoreID == storeId)
                .OrderByDescending(o => o.DraftDate)
                .ToListAsync();

            var orders = await query
                .Where(o => o.OrderStatusID == statusID && o.StoreID == storeId)
                .Include(o => o.SourceStorageArea)
                    .ThenInclude(wsa => wsa.Warehouse)
                .Include(o => o.DestinationStorageArea)
                    .ThenInclude(wsa => wsa.Warehouse)
                .Include(o => o.Supplier)
                .OrderByDescending(o => o.DraftDate)
                .ToListAsync();

            return Ok(orders.Select(o => o.OrderToOrderDto()));
        }

        [HttpPost("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(string orderID, int orderStatus)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderID == orderID);

            if (order == null)
            {
                return NotFound($"Order {orderID} not found.");
            }

            order.OrderStatusID = orderStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order Status updated successfully." });

        }

        [HttpPost("UpdateOrderItemStatus")]
        public async Task<IActionResult> UpdateOrderItemStatus(string orderID, int productItemID, int inputItemStatus)
        {               
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderID == orderID && oi.ProductItemID == productItemID);

            if (orderItem == null)
            {
                return NotFound($"Order item with ID {productItemID} for order {orderID} not found.");
            }

            orderItem.OrderItemStatusID = inputItemStatus;
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "OrderItem Status updated successfully." });

        }

        [HttpPost("draftProductMovement")]
        public async Task<IActionResult> DraftProductMovement(string orderID)
        {
            if (orderID == null)
            {
                return BadRequest("Invalid OrderID.");
            }

            var result = await _productMovementService.DraftProductMovementAsync(orderID);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500, result.ErrorMessage);
            }

            return Ok(new { Message = "ProductMovement drafted successfully.", OrderID = orderID, ProductMovementID = result.Data });
        }

        [HttpPost("submitProductMovement")]
        public async Task<IActionResult> SubmitProductMovement([FromBody] ProductMovementRequest request)
        {
            if (request == null || request.ProductMovementID <= 0)
            {
                return BadRequest("Invalid request. ProductMovementID is required.");
            }

            var result = await _productMovementService.SubmitProductMovementAsync(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500, result.ErrorMessage);
            }

            return Ok(new { Message = "ProductMovement submitted successfully.", ProductMovementID = request.ProductMovementID });
        }

        [HttpPost("updateProductMovementItem")]
        public async Task<IActionResult> UpdateProductMovementItem([FromBody] ProductMovementItemRequest request)
        {
            if (request == null || request.MovementID <= 0)
            {
                return BadRequest("Invalid request. ProductMovementID is required.");
            }

            var result = await _productMovementService.UpdateProductMovementItemAsync(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode ?? 500, result.ErrorMessage);
            }

            return Ok(new { Message = "ProductMovement Item updated successfully.", OrderID = request.OrderID, MovementID = request.MovementID, ProductItemID = request.ProductItemID });
        }
    }
}
