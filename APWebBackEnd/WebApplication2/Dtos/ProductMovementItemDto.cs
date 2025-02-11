using System;

namespace APWeb.Dtos
{
    public class ProductMovementItemDto
    {
        public string OrderID { get; set; }

        public int MovementID { get; set; }

        public int ProductItemID { get; set; }

        public int SourceWarehouseLocationID { get; set; }

        public int DestinationWarehouseLocationID { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitCost { get; set; }

        public decimal TaxRate { get; set; }

        public decimal QuantityRemaining { get; set; }

        public int ItemStatusID { get; set; }

        public DateTime DraftDate { get; set; }

        public DateTime SubmitDate { get; set; }

        public DateTime CompleteDate { get; set; }
    }
}
