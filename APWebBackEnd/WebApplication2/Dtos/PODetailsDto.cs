using System.ComponentModel.DataAnnotations;
using System;

namespace APWeb.Dtos
{
    public class PODetailsDto
    {
        public string POID { get; set; }
        public string ProductID  { get; set; }
        public int StoreID { get; set; }
        public int UnitsPerPackage { get; set; }
        public double? UnitsOrdered { get; set; }
        public double? PriceOrdered { get; set; }
        public int? UnitsReceived { get; set; }
        public Single? PriceReceived { get; set; }
        public double? TaxRate { get; set; }
        public string OrderedBy { get; set; }
        public DateTime? OrderingDate { get; set; }
        public DateTime? ReceivingDate { get; set; }
        [Key]
        public int TransactionID { get; set; }
        public int BuyerID { get; set; }
        public int SupplierID { get; set; }
        public string? Invoice { get; set; }
        public string? ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }

        public double StockUnitQty { get; set; }
        public string ProductName { get; set; }
    }
}
