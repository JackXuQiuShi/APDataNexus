namespace APWeb.Models
{
    public partial class Order
    {
        public virtual WarehouseStorageArea SourceStorageArea { get; set; } // Add this
        public virtual WarehouseStorageArea DestinationStorageArea { get; set; } // Add this
    }
}
