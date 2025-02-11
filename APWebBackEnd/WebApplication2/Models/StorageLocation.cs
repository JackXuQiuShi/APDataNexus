using System;

namespace APWeb.Models
{
    public class StorageLocation
    {
        public string ProductID { get; set; }
        public int StatusID { get; set; }
        public int StoreID { get; set; }
        public string Location { get; set; }
        public string LocationType { get; set; }
        public DateTime LatestDate { get; set; }

    }
}
