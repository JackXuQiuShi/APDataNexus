using System;
using System.ComponentModel.DataAnnotations;

namespace APWeb.Models
{
    public class HMRInventory
    {
        public string ProductID { get; set; }
        public string? UPC { get; set; }
        public string ProductName { get; set; }
        public double UnitQty { get; set; }
        public DateTime Date { get; set; }
        [Key]
        public int ID { get; set; }
    }
}
