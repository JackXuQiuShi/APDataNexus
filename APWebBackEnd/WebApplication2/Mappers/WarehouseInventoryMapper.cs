using APWeb.Dtos;
using APWeb.Models;
using System;
using System.Linq;

namespace APWeb.Mappers
{
    public static class WarehouseInventoryMapper
    {
        //     public static WarehouseDraft CreateWarehouseDraftDtoToDraft(this CreateWarehouseDraftDto record, ProductItem product)
        //     {
        //         /*return new WarehouseDraft
        //         {
        //             ProductID = record.UPC,
        //             ProductName = product.ProductName,
        //             Date = record.Date,
        //             UnitQty = record.UnitQty,
        //             Action = record.Action,
        //             SellTo = record.SellTo,
        //             StatusID = 0,
        //             StoreID = record.StoreID,
        //             SupplierID = record.SupplierID
        //         };*/
        //         throw new NotImplementedException();
        //     }

        //     public static WarehouseTransaction WarehouseDraftToTransaction(this WarehouseDraft draft, int PID)
        //     {

        //         /*return new WarehouseTransaction
        //         {
        //             PID = PID,
        //             UPC = draft.ProductID,
        //             UnitQty = draft.UnitQty,
        //             Date = draft.Date,
        //             Action = draft.Action,
        //             SellTo = draft.SellTo,
        //             SupplierID = draft.SupplierID,
        //             ProductName = draft.ProductName,
        //             StoreID = draft.StoreID

        //         };*/
        //         throw new NotImplementedException();
        //     }

        //     public static WarehouseInventory WarehouseDraftToInventory(this WarehouseDraft draft, int PID)
        //     {
        //         /* return new WarehouseInventory
        //          {
        //              UPC = draft.ProductID,
        //              UnitQty = draft.UnitQty,    
        //              Date = draft.Date,

        //              ProductName = draft.ProductName,
        //              PID = PID,
        //              StoreID = draft.StoreID
        //          };*/
        //         throw new NotImplementedException();
        //     }

        //     public static WarehouseTransaction AddInWarehouseDtoToTransaction(this AddInWarehouseDto adto, Product product, int? itemID)
        //     {

        //         return new WarehouseTransaction
        //         {
        //             ItemID = itemID,
        //             ProductID = adto.ProductID,
        //             UnitQty = adto.UnitQty,
        //             Date = adto.Date,
        //             Action = "Add In",
        //             SellTo = null,
        //             SupplierID = adto.SupplierID,
        //             ProductName = product.ProductName,
        //             StoreID = adto.StoreID,
        //             POID = adto.POID
        //         };
        //     }

        //     public static WarehouseInventory AddInWarehouseDtoToInventory(this AddInWarehouseDto adto, Product product, int? itemID)
        //     {
        //         return new WarehouseInventory
        //         {
        //             ProductID = adto.ProductID,
        //             UnitQty = adto.UnitQty,
        //             Date = adto.Date,
        //             SupplierID = adto.SupplierID,
        //             ProductName = product.ProductName,
        //             ItemID = itemID,
        //             StoreID = adto.StoreID
        //         };
        //     }


        //     public static AddInWarehouseDto DraftToAddInDto(this WarehouseDraft draft)
        //     {
        //         /* return new AddInWarehouseDto
        //          {
        //              UPC = draft.ProductID,
        //              UnitQty = draft.UnitQty,
        //              Date = draft.Date,
        //              SupplierID = draft.SupplierID,
        //              StoreID = draft.StoreID
        //          };*/
        //         throw new NotImplementedException();
        //     }


        //     public static WarehouseTransaction TakeOutWarehouseDtoToTransaction(this TakeOutWarehouseDto tdto, Product product, double UnitQty, int? itemID, int supplierId)
        //     {

        //         return new WarehouseTransaction
        //         {
        //             ItemID = itemID,
        //             ProductID = tdto.ProductID,
        //             UnitQty = UnitQty,
        //             Date = DateTime.Now,
        //             Action = "Take Out",
        //             SellTo = tdto.StoreID,
        //             SupplierID = supplierId,
        //             ProductName = product.ProductName,
        //             StoreID = tdto.Source,
        //             POID = tdto.POID
        //         };
        //     }


        //     public static TakeOutWarehouseDto DraftToTakeOutDto(this WarehouseDraft draft)
        //     {
        //         throw new NotImplementedException();
        //     }


        //     public static WarehouseTransaction AdjustWarehouseDtoToTransaction(this AdjustWarehouseInventoryDto adto, Product product, int? itemID, int SupplierID)
        //     {
        //         return new WarehouseTransaction
        //         {
        //             ItemID = itemID,
        //             ProductID = adto.ProductID,
        //             UnitQty = adto.UnitQty,
        //             Date = DateTime.Now,
        //             Action = "Adjustment",
        //             SellTo = null,
        //             SupplierID = SupplierID,
        //             ProductName = product.ProductName,
        //             StoreID = adto.StoreID,
        //             POID = null
        //         };
        //     }



    }
}
