using Microsoft.EntityFrameworkCore;

namespace APWeb.Models
{
    public partial class ApplicationDbContext
    {
        public DbSet<HMRInventory> HMRInventory { get; set; }
        public DbSet<HMRProduct> HMRProducts { get; set; }
        public DbSet<HMRTransaction> HMRTransactions { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // Add custom configurations here
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");
                entity.HasKey(d => d.DepartmentID);
                entity.Property(d => d.DepartmentName)
                      .HasMaxLength(25)
                      .IsUnicode(false);
            });

            modelBuilder.Entity<HMRProduct>(entity =>
            {
                entity.Property(e => e.UnitPrice)
                      .HasColumnType("decimal(18,2)"); // Adjust precision and scale as needed
            });

            // Invoices entity configuration
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");

                entity.HasKey(e => e.InvoiceID);

                entity.Property(e => e.InvoiceNumber)
                      .HasMaxLength(50)
                      .IsUnicode(false);
            });

            // InvoiceItems entity configuration
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.ToTable("InvoiceItems");

                entity.HasKey(e => e.InvoiceItemID);

                entity.Property(e => e.ProductID)
                      .HasMaxLength(15)
                      .IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
            });

            // Inventory entity configuration
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventory");

                entity.HasKey(e => e.InventoryID);

                entity.Property(e => e.CurrentStock).HasColumnType("decimal(10,2)");
            });


            // Order entity configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.OrderStatusID)
                      .HasColumnName("OrderStatusID")
                      .HasDefaultValue(1);

                entity.HasOne(o => o.OrderTypeNavigation)
                        .WithMany(ot => ot.Orders)
                        .HasForeignKey(o => o.OrderType)
                        .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(o => o.OrderItems)
                        .WithOne(oi => oi.Order)
                        .HasForeignKey(oi => oi.OrderID);
            });

            // OrderItems entity configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");

                entity.HasKey(e => new { e.OrderID, e.ProductItemID });

                entity.Property(e => e.UnitQty).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.ToTable("OrderType");
                entity.HasKey(e => e.OrderTypeID);
                entity.Property(e => e.OrderTypeName)
                      .HasMaxLength(50)
                      .IsUnicode(false)
                      .IsRequired();
            });

            modelBuilder.Entity<StorageLocation>(entity =>
            {
                entity.HasKey(l => new { l.ProductID, l.StoreID, l.Location, l.LocationType});
            });

            modelBuilder.Entity<PO>(entity =>
            {
                entity.ToTable("POs");

                entity.HasKey(e => e.PO_ID);

                entity.Property(e => e.PO_ID)
                      .HasMaxLength(255)
                      .IsUnicode(true);

                entity.Property(e => e.State)
                      .HasMaxLength(255)
                      .IsUnicode(true);

                // Configure other columns as needed.
            });

            modelBuilder.Entity<PO_Detail>(entity =>
            {
                entity.ToTable("PO_Details");

                entity.HasKey(e => new { e.PO_ID, e.Product_ID, e.Store_ID });

                entity.Property(e => e.PO_ID)
                      .HasMaxLength(15)
                      .IsUnicode(false);

                entity.Property(e => e.Product_ID)
                      .HasMaxLength(15)
                      .IsUnicode(false);

                // Configure other columns as needed.
            });

            // Product entity configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(e => new { e.ProductID });
                
                entity.Property(e => e.ProductID)
                      .HasMaxLength(15)
                      .IsUnicode(false);

                entity.Property(e => e.Unit_Cost).HasColumnType("money");
                entity.Property(e => e.SuggestSalePrice).HasColumnType("money");
            });

            // ProductItems entity configuration
            modelBuilder.Entity<ProductItem>(entity =>
            {
                entity.ToTable("ProductItems");

                entity.HasKey(e => e.ItemID);

                entity.Property(e => e.ProductID)
                      .HasMaxLength(15)
                      .IsUnicode(false);

                entity.Property(e => e.UnitCost).HasColumnType("money");
            });

            // Products_Store entity configuration
            modelBuilder.Entity<Products_Store>(entity =>
            {
                entity.ToTable("Products_Store");

                entity.HasKey(e => new { e.ProductID, e.Store_ID });

                entity.Property(e => e.ProductID)
                      .HasMaxLength(15)
                      .IsUnicode(false);

                entity.Property(e => e.Store_ID).IsRequired();
            });

            // ProductMovement entity configuration
            modelBuilder.Entity<ProductMovement>(entity =>
            {
                entity.ToTable("ProductMovement");
                entity.HasKey(e => new { e.OrderID, e.MovementID });
                entity.Property(e => e.MovementStatusID).HasColumnName("MovementStatusID");
                entity.Property(e => e.TotalCost).HasColumnName("TotalCost").HasColumnType("decimal(10,2)");
                entity.Property(e => e.DraftDate).HasDefaultValueSql("(getdate())");
            });

            // ProductMovementItem entity configuration
            modelBuilder.Entity<ProductMovementItem>(entity =>
            {
                entity.ToTable("ProductMovementItems");
                entity.HasKey(e => new { e.OrderID, e.MovementID, e.ProductItemID });
                entity.HasOne(pmi => pmi.ProductMovement)
                        .WithMany(pm => pm.ProductMovementItems)
                        .HasForeignKey(pmi => new { pmi.OrderID, pmi.MovementID });

                entity.HasOne(pmi => pmi.OrderItem)
                    .WithMany(oi => oi.ProductMovementItems)
                    .HasForeignKey(pmi => new { pmi.OrderID, pmi.ProductItemID });
            });

            // StoreFloorLocation entity configuration
            modelBuilder.Entity<StoreFloorLocation>(entity =>
            {
                entity.ToTable("StoreFloorLocation");

                entity.HasKey(e => e.StoreLocationID);

                entity.Property(e => e.Location)
                      .HasMaxLength(15)
                      .IsUnicode(false);
            });

            // StoreFloorLocationType entity configuration
            modelBuilder.Entity<StoreFloorLocationType>(entity =>
            {
                entity.ToTable("StoreFloorLocationType");

                entity.HasKey(e => e.LocationTypeID);

                entity.Property(e => e.LocationTypeDesc)
                      .HasMaxLength(50)
                      .IsUnicode(false);
            });

            // Stores entity configuration
            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Stores");

                entity.HasKey(e => e.Store_ID);

                entity.Property(e => e.StoreName)
                      .HasMaxLength(40)
                      .IsUnicode(true);
            });

            // Add configurations for other tables like ProductItems, Suppliers, etc.
            // Example:
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Suppliers");

                entity.HasKey(e => e.Supplier_ID);

                entity.Property(e => e.CompanyName)
                      .HasMaxLength(255)
                      .IsUnicode(true);
            });

            // Warehouses entity configuration
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.ToTable("Warehouses");

                entity.HasKey(e => e.WarehouseID);

                entity.Property(e => e.WarehouseName)
                      .HasMaxLength(100)
                      .IsUnicode(false);

                entity.Property(e => e.Location)
                      .HasMaxLength(255)
                      .IsUnicode(false);
            });

            // WarehouseStorageAreas entity configuration
            modelBuilder.Entity<WarehouseStorageArea>(entity =>
            {
                entity.ToTable("WarehouseStorageAreas");

                entity.HasKey(e => e.WarehouseStorageAreaID);

                entity.Property(e => e.AreaType)
                      .HasMaxLength(50)
                      .IsUnicode(false);
            });

            // WarehouseStorageLocations entity configuration
            modelBuilder.Entity<WarehouseStorageLocation>(entity =>
            {
                entity.ToTable("WarehouseStorageLocations");

                entity.HasKey(e => e.WarehouseLocationID);

                entity.Property(e => e.LocationName)
                      .HasMaxLength(50)
                      .IsUnicode(false);
            });

            // Add more custom configurations here
        }
    }
}

