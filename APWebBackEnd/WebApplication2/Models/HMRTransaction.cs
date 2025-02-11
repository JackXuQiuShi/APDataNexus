using System;
using System.ComponentModel.DataAnnotations;

namespace APWeb.Models
{
    public class HMRTransaction
    {
        public string ProductID { get; set; }  // nchar(15) - Not Nullable
        public string? UPC { get; set; }  // nchar(15) - Not Nullable
        public double UnitQty { get; set; }  // float - Not Nullable
        public DateTime Date { get; set; }  // date - Not Nullable
        public string? Action { get; set; }  // date - Not Nullable
        public string? SellTo { get; set; }  // nvarchar(20) - Nullable
        [Key]
        public int TransactionID { get; set; }  // int - Not Nullable
        public string? ProductName { get; set; }
        public DateTime? ReceivingDate { get; set; }
    }
}
