using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APWeb.Models
{
    public class PODetails
    {
        [ForeignKey("PO")]
        public string PO_ID { get; set; }
        public string Product_ID { get; set; }
        public int Store_ID { get; set; }
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
        public int Transaction_ID { get; set; }
        public PO PO { get; set; }
        //
        //
        //public int Buyer_ID { get; set; }
        //public int Supplier_ID { get; set; }
        //public DateTime PODraftDate { get; set; }
        //public int POComplete { get; set; }
        //public DateTime DeliveryDate { get; set; }
        //public string DeliveryTo { get; set; }
        //public string Invoice { get; set; }
        //public string State { get; set; }
        //public string Ordered_By { get; set; }
        //public string Received_By { get; set; }
        //public DateTime Ordering_Date { get; set; }
        //public DateTime Ordered_Date { get; set; }
        //public DateTime Received_Date { get; set; }
        //public DateTime Invoice_Date { get; set; }

    }
}
