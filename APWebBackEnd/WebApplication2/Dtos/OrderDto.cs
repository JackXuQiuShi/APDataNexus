
using System;
using System.Collections.Generic;

namespace APWeb.Dtos
{
    public class OrderDto
    {
        public string OrderID { get; set; }
        public DateTime DraftDate { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public int OrderType { get; set; }
        public int? SourceStorageAreaID { get; set; }
        public int DestinationStorageAreaID { get; set; }
        public int? SupplierID { get; set; }
        public int? StoreID { get; set; }
        public string SourceWarehouseName { get; set; }
        public string DestinationWarehouseName { get; set; }
        public int OrderStatusID { get; set; }
        public string SupplierName { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }


    }
}