using APWeb.Models;

namespace APWeb.Dtos
{
    public class OrderItemDto
    {
        
        public string OrderID { get; set; }
        public int ProductItemID { get; set; }
        public string ProductID { get; set; }
        public int SourceWarehouseLocationID { get; set; }
        public int DestinationWarehouseLocationID { get; set; }
        public decimal UnitQty { get; set; }
        public decimal UnitCost { get; set; }
        public string ProductName { get; set; }
        public int UnitsPerPackage { get; set; }
        public decimal TaxRate { get; set; }
        public int OrderItemStatusID { get; set; }
        public decimal SourceCurrentStock { get; set; }   
        public decimal DestinationCurrentStock { get; set; }        
        public string Location { get; set; }
        //public List<ProductMovementItemDto> ProductMovementItems { get; set; }
        public decimal MovementID { get; set; }
        public int ProductMovementSourceWarehouseLocationID { get; set; }
        public int ProductMovementDestinationWarehouseLocationID { get; set; }
        public decimal ProductMovementUnitCost { get; set; }
        public decimal ProductMovementQuantityRemaining { get; set; }
        public int ProductMovementItemStatusID { get; set; }
        public decimal ProductMovementUnitQty { get; set; }

    }
}
