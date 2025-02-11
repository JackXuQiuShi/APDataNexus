using System.Collections.Generic;

namespace APWeb.Models
{
    public partial class StorageArea
    {
        public int StorageAreaID { get; set; }

        public string Name { get; set; }

        public int WarehouseID { get; set; }

        // Add this navigation property
        public virtual Warehouse Warehouse { get; set; }

        public virtual ICollection<Order> SourceOrders { get; set; } = new List<Order>();

        public virtual ICollection<Order> DestinationOrders { get; set; } = new List<Order>();
    }
}
