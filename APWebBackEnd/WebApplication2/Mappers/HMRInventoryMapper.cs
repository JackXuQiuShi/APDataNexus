using APWeb.Dtos;
using APWeb.Models;
using System;

namespace APWeb.Mappers
{
    public static class HMRInventoryMapper
    {
        public static HMRInventory AddInDtoToHMRInventory(this AddInHMRDto inventory, HMRProduct product)
        {
            return new HMRInventory
            {
                ProductID = inventory.ProductID,
                UPC = product.UPC,
                UnitQty = inventory.UnitQty,
                Date = inventory.Date,
                ProductName = product.ProductName
            };
        }

        public static HMRTransaction AddInDtoToHMRTransactions(this AddInHMRDto inventory, HMRProduct product)
        {
            return new HMRTransaction
            {
                ProductID = inventory.ProductID,
                UPC = product.UPC,
                UnitQty = inventory.UnitQty,
                Date = inventory.Date,
                Action = "Add In",
                ProductName = product.ProductName
            };
        }

        public static HMRTransaction TakeOutDtoToHMRTransactions(this TakeOutHMRDto inventory, HMRProduct product, double unitQty, DateTime inventoryDate)
        {
            return new HMRTransaction
            {
                ProductID = inventory.ProductID,
                UPC = product.UPC,
                UnitQty = unitQty,
                Date = DateTime.Now,
                Action = "Take Out",
                ProductName = product.ProductName,
                ReceivingDate = inventoryDate,
                SellTo = inventory.SellTo
            };
        }

        public static HMRProductDto HMRProductToDto(this HMRProduct product)
        {
            return new HMRProductDto
            {
                ProductID = product.ProductID,
                UPC = product.UPC,
                ProductName = product.ProductName,
                ProductType = product.ProductType,
                Department = product.Department,
                UnitOfMeasure = product.UnitOfMeasure,
                ProductDesc = product.ProductDesc,
                SupplierID = product.SupplierID,
                Location = product.Location,
                SupplierRefNum = product.SupplierRefNum,
                UnitPrice = product.UnitPrice,
                SupplierName = product.Supplier?.CompanyName ?? string.Empty
            };
        }
    } 
}
