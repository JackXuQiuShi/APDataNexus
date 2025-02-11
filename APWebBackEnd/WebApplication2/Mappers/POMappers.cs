using APWeb.Dtos;
using APWeb.Models;
using System.Linq;

namespace APWeb.Mappers
{
    public static class POMappers
    {
        public static PODetailsDto PODetailsToDto(this PODetails pd)
        {
            return new PODetailsDto
            {
                POID = pd.PO_ID,
                ProductID = pd.Product_ID,
                StoreID = pd.Store_ID,
                UnitsPerPackage = pd.UnitsPerPackage,
                UnitsOrdered = pd.UnitsOrdered,
                PriceOrdered = pd.PriceOrdered,
                UnitsReceived = pd.UnitsReceived,
                PriceReceived = pd.PriceReceived,
                TaxRate = pd.TaxRate,
                OrderedBy = pd.OrderedBy,
                OrderingDate = pd.OrderingDate,
                ReceivingDate = pd.ReceivingDate,
                TransactionID = pd.Transaction_ID,
                BuyerID = pd.PO.Buyer_ID,
                SupplierID = pd.PO.Supplier_ID,
                Invoice = pd.PO.Invoice,
                ReceivedBy = pd.PO.Received_By,
                ReceivedDate = pd.PO.Received_Date
            };
        }

        public static OrderDto OrderToOrderDto(this Order orderEntity)
        {
            return new OrderDto
            {
                OrderID = orderEntity.OrderID,
                DraftDate = orderEntity.DraftDate,
                SubmitDate = orderEntity.SubmitDate,
                CompleteDate = orderEntity.CompleteDate,
                OrderType = orderEntity.OrderType,
                SourceStorageAreaID = orderEntity.SourceStorageAreaID,
                DestinationStorageAreaID = orderEntity.DestinationStorageAreaID,
                SupplierID = orderEntity.SupplierID,
                StoreID = orderEntity.StoreID,
                OrderStatusID = orderEntity.OrderStatusID,
                SourceWarehouseName = orderEntity.SourceStorageArea?.WarehouseStorageAreaName,
                DestinationWarehouseName = orderEntity.DestinationStorageArea?.WarehouseStorageAreaName,
                SupplierName = orderEntity.Supplier?.CompanyName,
                OrderItems = orderEntity.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductItemID = oi.ProductItemID,
                    ProductID = oi.ProductItem?.ProductID,
                    OrderID = oi.OrderID,
                    SourceWarehouseLocationID = oi.SourceWarehouseLocationID,
                    DestinationWarehouseLocationID = oi.DestinationWarehouseLocationID,
                    UnitQty = oi.UnitQty,
                    UnitCost = oi.UnitCost,
                    ProductName = oi.ProductItem?.ItemDesc,
                    UnitsPerPackage = oi.UnitsPerPackage,
                    TaxRate = oi.TaxRate,
                    OrderItemStatusID = oi.OrderItemStatusID,
                }).ToList()
            };
        }

        public static OrderItemDto OrderItemToOrderItemDto(this OrderItem oi)
        {
            return new OrderItemDto
            {

                OrderID = oi.OrderID,
                ProductItemID = oi.ProductItemID,
                ProductID = oi.ProductItem?.ProductID,
                SourceWarehouseLocationID = oi.SourceWarehouseLocationID,
                DestinationWarehouseLocationID = oi.DestinationWarehouseLocationID,
                UnitQty = oi.UnitQty,
                UnitCost = oi.UnitCost,
                ProductName = oi.ProductItem?.ItemDesc,
                UnitsPerPackage = oi.UnitsPerPackage,
                TaxRate= oi.TaxRate,
                OrderItemStatusID = oi.OrderItemStatusID,
            };
        }

        public static ProductMovementItemDto ProductMovementItemToProductMovementItemDto(this ProductMovementItem pmi)
        {
            return new ProductMovementItemDto
            {
                OrderID = pmi.OrderID,
                MovementID = pmi.MovementID,
                ProductItemID = pmi.ProductItemID,
                SourceWarehouseLocationID = pmi.SourceWarehouseLocationID,
                DestinationWarehouseLocationID = pmi.DestinationWarehouseLocationID,
                Quantity = pmi.UnitQty,
                UnitCost = pmi.UnitCost,
                TaxRate = pmi.TaxRate,
                QuantityRemaining = pmi.QuantityRemaining,
                ItemStatusID = pmi.ItemStatusID,
                DraftDate = pmi.DraftDate,
                SubmitDate = pmi.SubmitDate,
                CompleteDate = pmi.CompleteDate
            };
        }
    }
}
