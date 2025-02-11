using System;

namespace APWeb.Models
{
    public class Item
    {
        public int RequestStoreID { get; set; }
        public string Applicant { get; set; }
        public int DepartmentID { get; set; }
        public string ProductID { get; set; }
        public int CheckDigit { get; set; }
        public string ProductFullName { get; set; }
        public string ProductName { get; set; }
        public string ProductAlias { get; set; }
        public string PackageSpec { get; set; }
        public string Measure { get; set; }
        public int NumPerPack { get; set; }
        public int Tax1App { get; set; }
        public int Tax2App { get; set; }
        public decimal UnitCost { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public decimal VolumeUnitCost { get; set; }
        public decimal MinBulkVolume { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Ethnic { get; set; }
        public int SupplierID { get; set; }
        public decimal UnitSize { get; set; }
        public string UnitSizeUom { get; set; }
        public int BuyerID { get; set; }

    }
}
