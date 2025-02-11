using System;
using System.Collections.Generic;

namespace APWeb.Dtos
{
    public class ReceiveOrderDto
    {
        public string OrderID { get; set; }
        public int MovementType { get; set; }
        public List<ReceiveOrderItemDto> OrderItem { get; set; }
    }

    public class ReceiveOrderItemDto
    {
        public int ProductItemID { get; set; }
        public int SourceWarehouseLocationID { get; set; }
        public int DestinationWarehouseLocationID { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal ReceivedCost { get; set; }
        public int ItemID { get; set; } // assuming it’s a field in your WarehouseInventory table
    }
}
