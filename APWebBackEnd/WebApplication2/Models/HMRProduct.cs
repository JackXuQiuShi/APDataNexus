using System.ComponentModel.DataAnnotations;

namespace APWeb.Models
{
    public class HMRProduct
    {
        [Key]
        public string ProductID { get; set; }  // nchar(15) - Not Nullable

        public string? UPC { get; set; }  // nchar(15) - Nullable

        public string ProductName { get; set; }  // nvarchar(50) - Nullable

        public string ProductType { get; set; }  // nvarchar(20) - Nullable

        public string Department { get; set; }

        public string UnitOfMeasure { get; set; }  // nchar(6) - Nullable

        public string? ProductDesc { get; set; }  // nvarchar(50) - Nullable

        public int? SupplierID { get; set; }  // int - Nullable

        public string? Location { get; set; }  // nvarchar(20) - Nullable

        public string? SupplierRefNum { get; set; }  // nvarchar(30) - Nullable

        public decimal UnitPrice { get; set; }
        
        public Supplier? Supplier { get; set; }
    }

}

