using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WETT.Data
{
    public partial class WETT_DBContext : DbContext
    {
        public WETT_DBContext()
        {
        }

        public WETT_DBContext(DbContextOptions<WETT_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CallFrequency> CallFrequencies { get; set; }
        public virtual DbSet<Carrier> Carriers { get; set; }
        public virtual DbSet<Cdo> Cdos { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<CustomerOrderDetail> CustomerOrderDetails { get; set; }
        public virtual DbSet<CustomerOrderStatus> CustomerOrderStatuses { get; set; }
        public virtual DbSet<CustomerSource> CustomerSources { get; set; }
        public virtual DbSet<CustomerStatus> CustomerStatuses { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryLocation> InventoryLocations { get; set; }
        public virtual DbSet<InventoryTx> InventoryTxes { get; set; }
        public virtual DbSet<InventoryTxDetail> InventoryTxDetails { get; set; }
        public virtual DbSet<InventoryTxReason> InventoryTxReasons { get; set; }
        public virtual DbSet<MbllCustomer> MbllCustomers { get; set; }
        public virtual DbSet<MbllSalesOrder> MbllSalesOrders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Segment> Segments { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierOrder> SupplierOrders { get; set; }
        public virtual DbSet<SupplierOrderDetail> SupplierOrderDetails { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:wett-dev-db.database.windows.net;Authentication=Active Directory Default;Database=WETT_DEV;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CallFrequency>(entity =>
            {
                entity.ToTable("Call Frequency");

                entity.Property(e => e.CallFrequencyId).HasColumnName("Call Frequency Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Carrier>(entity =>
            {
                entity.ToTable("Carrier");

                entity.Property(e => e.CarrierId).HasColumnName("Carrier Id");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Address2).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100)
                    .HasColumnName("Contact Name");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(20)
                    .HasColumnName("Contact Phone");

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(12)
                    .HasColumnName("Postal Code");

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Update UserId")
                    .HasDefaultValueSql("(user_name())");
            });

            modelBuilder.Entity<Cdo>(entity =>
            {
                entity.HasKey(e => e.CdosId);

                entity.ToTable("CDOS");

                entity.Property(e => e.CdosId)
                    .ValueGeneratedNever()
                    .HasColumnName("CDOS Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId).HasColumnName("Customer Id");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.CallFrequencyId).HasColumnName("Call Frequency Id");

                entity.Property(e => e.CdosId).HasColumnName("CDOS Id");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(100)
                    .HasColumnName("Contact Email");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100)
                    .HasColumnName("Contact Name");

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CustomerSourceId).HasColumnName("Customer Source Id");

                entity.Property(e => e.CustomerStatusCode)
                    .HasMaxLength(1)
                    .HasColumnName("Customer Status Code")
                    .IsFixedLength();

                entity.Property(e => e.CustomerTypeCode)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("Customer Type Code")
                    .IsFixedLength();

                entity.Property(e => e.DeletedDate)
                    .HasPrecision(0)
                    .HasColumnName("Deleted Date");

                entity.Property(e => e.DeletedFlag).HasColumnName("Deleted Flag");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LicenceNumber)
                    .HasMaxLength(10)
                    .HasColumnName("Licence Number")
                    .IsFixedLength();

                entity.Property(e => e.MbllCustomerNo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("MBLL Customer No");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone1)
                    .HasMaxLength(50)
                    .HasColumnName("Phone 1");

                entity.Property(e => e.Phone1Type)
                    .HasMaxLength(50)
                    .HasColumnName("Phone 1 Type");

                entity.Property(e => e.Phone2)
                    .HasMaxLength(50)
                    .HasColumnName("Phone 2");

                entity.Property(e => e.Phone2Type)
                    .HasMaxLength(50)
                    .HasColumnName("Phone 2 Type");

                entity.Property(e => e.Phone3)
                    .HasMaxLength(50)
                    .HasColumnName("Phone 3");

                entity.Property(e => e.Phone3Type)
                    .HasMaxLength(50)
                    .HasColumnName("Phone 3 Type");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(50)
                    .HasColumnName("Postal Code");

                entity.Property(e => e.Province)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.SegmentId).HasColumnName("Segment Id");

                entity.Property(e => e.TerritoryId).HasColumnName("Territory Id");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Update UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.CallFrequency)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CallFrequencyId)
                    .HasConstraintName("FK_Customer_Call Frequency");

                entity.HasOne(d => d.Cdos)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CdosId)
                    .HasConstraintName("FK_Customer_CDOS");

                entity.HasOne(d => d.CustomerSource)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CustomerSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Customer Source");

                entity.HasOne(d => d.CustomerStatusCodeNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CustomerStatusCode)
                    .HasConstraintName("FK_Customer_Customer Status");

                entity.HasOne(d => d.CustomerTypeCodeNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CustomerTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Customer Type");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.SegmentId)
                    .HasConstraintName("FK_Customer_Segment");

                entity.HasOne(d => d.Territory)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.TerritoryId)
                    .HasConstraintName("FK_Customer_Territory");
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.ToTable("Customer Order");

                entity.Property(e => e.CustomerOrderId).HasColumnName("Customer Order Id");

                entity.Property(e => e.CarrierId).HasColumnName("Carrier Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer Id");

                entity.Property(e => e.CustomerOrderStatusId).HasColumnName("Customer Order Status Id");

                entity.Property(e => e.DateOrdered)
                    .HasColumnType("date")
                    .HasColumnName("Date Ordered");

                entity.Property(e => e.DeliveryReqDate)
                    .HasColumnType("date")
                    .HasColumnName("Delivery Req Date");

                entity.Property(e => e.Driver).HasMaxLength(50);

                entity.Property(e => e.DsSlipNumber)
                    .HasMaxLength(50)
                    .HasColumnName("DS Slip Number");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Order Number");

                entity.Property(e => e.SpecialInstructions).HasColumnName("Special Instructions");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Update UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Carrier)
                    .WithMany(p => p.CustomerOrders)
                    .HasForeignKey(d => d.CarrierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer Order_Carrier");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer Order_Customer");

                entity.HasOne(d => d.CustomerOrderStatus)
                    .WithMany(p => p.CustomerOrders)
                    .HasForeignKey(d => d.CustomerOrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer Order_Customer Order Status");
            });

            modelBuilder.Entity<CustomerOrderDetail>(entity =>
            {
                entity.ToTable("Customer Order Detail");

                entity.Property(e => e.CustomerOrderDetailId).HasColumnName("Customer Order Detail Id");

                entity.Property(e => e.CustomerOrderId).HasColumnName("Customer Order Id");

                entity.Property(e => e.QtyFulfilled).HasColumnName("Qty Fulfilled");

                entity.Property(e => e.QtyOrdered).HasColumnName("Qty Ordered");

                entity.HasOne(d => d.CustomerOrder)
                    .WithMany(p => p.CustomerOrderDetails)
                    .HasForeignKey(d => d.CustomerOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer Order Detail_Customer Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomerOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer Order Detail_Product");
            });

            modelBuilder.Entity<CustomerOrderStatus>(entity =>
            {
                entity.ToTable("Customer Order Status");

                entity.Property(e => e.CustomerOrderStatusId).HasColumnName("Customer Order Status Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerSource>(entity =>
            {
                entity.ToTable("Customer Source");

                entity.Property(e => e.CustomerSourceId).HasColumnName("Customer Source Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerStatus>(entity =>
            {
                entity.HasKey(e => e.CustomerStatusCode);

                entity.ToTable("Customer Status");

                entity.Property(e => e.CustomerStatusCode)
                    .HasMaxLength(1)
                    .HasColumnName("Customer Status Code")
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeCode);

                entity.ToTable("Customer Type");

                entity.Property(e => e.CustomerTypeCode)
                    .HasMaxLength(1)
                    .HasColumnName("Customer Type Code")
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("Inventory");

                entity.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.UseHistoryTable("MSSQL_TemporalHistoryFor_466100701", "dbo");
                    ttb
                        .HasPeriodStart("SysStartTime")
                        .HasColumnName("SysStartTime");
                    ttb
                        .HasPeriodEnd("SysEndTime")
                        .HasColumnName("SysEndTime");
                }
));

                entity.Property(e => e.ProductId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Product Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.InventoryLocationId).HasColumnName("Inventory Location Id");

                entity.HasOne(d => d.InventoryLocation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.InventoryLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Inventory Location");

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.Inventory)
                    .HasForeignKey<Inventory>(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Product");
            });

            modelBuilder.Entity<InventoryLocation>(entity =>
            {
                entity.ToTable("Inventory Location");

                entity.Property(e => e.InventoryLocationId).HasColumnName("Inventory Location Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<InventoryTx>(entity =>
            {
                entity.ToTable("Inventory Tx");

                entity.Property(e => e.InventoryTxId).HasColumnName("Inventory Tx Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.InventoryTxReasonId).HasColumnName("Inventory Tx Reason Id");

                entity.Property(e => e.UpdateTimestamp)
                    .HasColumnName("Update Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Update UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.InventoryTxReason)
                    .WithMany(p => p.InventoryTxes)
                    .HasForeignKey(d => d.InventoryTxReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory Tx_Inventory Tx Reason");
            });

            modelBuilder.Entity<InventoryTxDetail>(entity =>
            {
                entity.ToTable("Inventory Tx Detail");

                entity.Property(e => e.InventoryTxDetailId).HasColumnName("Inventory Tx Detail Id");

                entity.Property(e => e.InventoryLocationId).HasColumnName("Inventory Location Id");

                entity.Property(e => e.InventoryTxId).HasColumnName("Inventory Tx Id");

                entity.Property(e => e.ProductId).HasColumnName("Product Id");

                entity.HasOne(d => d.InventoryLocation)
                    .WithMany(p => p.InventoryTxDetails)
                    .HasForeignKey(d => d.InventoryLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory Tx Detail_Inventory Location");

                entity.HasOne(d => d.InventoryTx)
                    .WithMany(p => p.InventoryTxDetails)
                    .HasForeignKey(d => d.InventoryTxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory Tx Detail_Inventory Tx");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryTxDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory Tx Detail_Product");
            });

            modelBuilder.Entity<InventoryTxReason>(entity =>
            {
                entity.ToTable("Inventory Tx Reason");

                entity.Property(e => e.InventoryTxReasonId).HasColumnName("Inventory Tx Reason Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MbllCustomer>(entity =>
            {
                entity.HasKey(e => new { e.CustomerType, e.CustomerNumber });

                entity.ToTable("MBLL Customer");

                entity.Property(e => e.CustomerType)
                    .HasMaxLength(10)
                    .HasColumnName("Customer Type")
                    .IsFixedLength();

                entity.Property(e => e.CustomerNumber)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("Customer Number");

                entity.Property(e => e.ChangeType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Change Type")
                    .IsFixedLength();

                entity.Property(e => e.CustomerStatus)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Customer Status")
                    .IsFixedLength();

                entity.Property(e => e.InsertTimestamp)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Municipality).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(50)
                    .HasColumnName("Postal Code");

                entity.Property(e => e.PremiseName)
                    .HasMaxLength(50)
                    .HasColumnName("Premise Name");

                entity.Property(e => e.Province)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.StreetAddress)
                    .HasMaxLength(50)
                    .HasColumnName("Street Address");
            });

            modelBuilder.Entity<MbllSalesOrder>(entity =>
            {
                entity.HasKey(e => e.TrxNumber);

                entity.ToTable("MBLL Sales Order");

                entity.Property(e => e.TrxNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Trx Number");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("Create Timestamp");

                entity.Property(e => e.CustomerNumber)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("Customer Number");

                entity.Property(e => e.CustomerType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Customer Type")
                    .IsFixedLength();

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("Delivery Date");

                entity.Property(e => e.DeliveryInstructions)
                    .HasMaxLength(50)
                    .HasColumnName("Delivery Instructions");

                entity.Property(e => e.DeliveryTime)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("Delivery Time");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ItemNumber)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("Item Number");

                entity.Property(e => e.OrderType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Order Type")
                    .IsFixedLength();

                entity.Property(e => e.Quantity).HasColumnType("numeric(8, 0)");

                entity.Property(e => e.TrxDate)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("Trx Date");

                entity.Property(e => e.TrxType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Trx Type");

                entity.Property(e => e.Upc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("UPC");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("Product Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SKU");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier Id");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Update UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Supplier");
            });

            modelBuilder.Entity<Segment>(entity =>
            {
                entity.ToTable("Segment");

                entity.Property(e => e.SegmentId)
                    .ValueGeneratedNever()
                    .HasColumnName("Segment Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier Id");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Address2).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100)
                    .HasColumnName("Contact Name");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(20)
                    .HasColumnName("Contact Phone");

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(12)
                    .HasColumnName("Postal Code");

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Update UserId")
                    .HasDefaultValueSql("(user_name())");
            });

            modelBuilder.Entity<SupplierOrder>(entity =>
            {
                entity.ToTable("Supplier Order");

                entity.Property(e => e.SupplierOrderId).HasColumnName("Supplier Order Id");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("Order Date");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier Id");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Update UserId")
                    .HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierOrders)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier Order_Supplier");
            });

            modelBuilder.Entity<SupplierOrderDetail>(entity =>
            {
                entity.ToTable("Supplier Order Detail");

                entity.Property(e => e.SupplierOrderDetailId).HasColumnName("Supplier Order Detail Id");

                entity.Property(e => e.InventoryLocationId).HasColumnName("Inventory Location Id");

                entity.Property(e => e.ProductId).HasColumnName("Product Id");

                entity.Property(e => e.SupplierOrderId).HasColumnName("Supplier Order Id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SupplierOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier Order Detail_Product");

                entity.HasOne(d => d.SupplierOrder)
                    .WithMany(p => p.SupplierOrderDetails)
                    .HasForeignKey(d => d.SupplierOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier Order Detail_Supplier Order");
            });

            modelBuilder.Entity<Territory>(entity =>
            {
                entity.ToTable("Territory");

                entity.Property(e => e.TerritoryId).HasColumnName("Territory Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
