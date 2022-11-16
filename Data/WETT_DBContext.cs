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
        public virtual DbSet<InventoryTxType> InventoryTxTypes { get; set; }
        public virtual DbSet<MbllCustomer> MbllCustomers { get; set; }
        public virtual DbSet<MbllSalesOrder> MbllSalesOrders { get; set; }
        public virtual DbSet<OrderSource> OrderSources { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Segment> Segments { get; set; }
        public virtual DbSet<ShippingLocation> ShippingLocations { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }
        public virtual DbSet<TruckingCompany> TruckingCompanies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:WETTDbConnection");
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
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(12)
                    .HasColumnName("Postal Code");

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update UserId");
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
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

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
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update UserId");

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
                    .HasPrecision(0)
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
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Order Number");

                entity.Property(e => e.OrderSourceId).HasColumnName("Order Source Id");

                entity.Property(e => e.SpecialInstructions).HasColumnName("Special Instructions");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update UserId");

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

                entity.HasOne(d => d.OrderSource)
                    .WithMany(p => p.CustomerOrders)
                    .HasForeignKey(d => d.OrderSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer Order_Order Source");
            });

            modelBuilder.Entity<CustomerOrderDetail>(entity =>
            {
                entity.ToTable("Customer Order Detail");

                entity.Property(e => e.CustomerOrderDetailId).HasColumnName("Customer Order Detail Id");

                entity.Property(e => e.CustomerOrderId).HasColumnName("Customer Order Id");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.Notes).HasMaxLength(100);

                entity.Property(e => e.ProductId).HasColumnName("Product Id");

                entity.Property(e => e.QtyFulfilled).HasColumnName("Qty Fulfilled");

                entity.Property(e => e.QtyOrdered).HasColumnName("Qty Ordered");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserid)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update Userid");

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
                entity.HasKey(e => new { e.ProductId, e.InventoryLocationId });

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

                entity.Property(e => e.ProductId).HasColumnName("Product Id");

                entity.Property(e => e.InventoryLocationId).HasColumnName("Inventory Location Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.InventoryTxDetailId).HasColumnName("Inventory Tx Detail Id");

                entity.HasOne(d => d.InventoryLocation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.InventoryLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Inventory Location");

                entity.HasOne(d => d.InventoryTxDetail)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.InventoryTxDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Tx");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.ProductId)
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

                entity.HasIndex(e => e.StockAdjCode, "IX_Stock_Adj_Code")
                    .IsUnique();

                entity.Property(e => e.InventoryTxId).HasColumnName("Inventory Tx Id");

                entity.Property(e => e.Date).HasPrecision(0);

                entity.Property(e => e.FromInventoryLocationId).HasColumnName("From Inventory Location Id");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.InventoryTxTypeId).HasColumnName("Inventory Tx Type Id");

                entity.Property(e => e.PortOfEntry)
                    .HasMaxLength(50)
                    .HasColumnName("Port of Entry");

                entity.Property(e => e.PreviousTransactionNo)
                    .HasMaxLength(50)
                    .HasColumnName("Previous Transaction No");

                entity.Property(e => e.Probill).HasMaxLength(50);

                entity.Property(e => e.PurchaseOrder)
                    .HasMaxLength(20)
                    .HasColumnName("Purchase Order");

                entity.Property(e => e.Seal).HasMaxLength(20);

                entity.Property(e => e.ShippingLocationId)
                    .HasColumnName("Shipping Location Id")
                    .IsSparse();

                entity.Property(e => e.StockAdjCode)
                    .HasMaxLength(20)
                    .HasColumnName("Stock Adj Code");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("Supplier Id")
                    .IsSparse();

                entity.Property(e => e.ToInventoryLocationId).HasColumnName("To Inventory Location Id");

                entity.Property(e => e.TransactionNo)
                    .HasMaxLength(50)
                    .HasColumnName("Transaction No");

                entity.Property(e => e.TruckingCompanyId)
                    .HasColumnName("Trucking Company Id")
                    .IsSparse();

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update UserId");

                entity.HasOne(d => d.FromInventoryLocation)
                    .WithMany(p => p.InventoryTxFromInventoryLocations)
                    .HasForeignKey(d => d.FromInventoryLocationId)
                    .HasConstraintName("FK_Inventory Tx_From Inventory Location");

                entity.HasOne(d => d.InventoryTxType)
                    .WithMany(p => p.InventoryTxes)
                    .HasForeignKey(d => d.InventoryTxTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory Tx_Inventory Tx Type");

                entity.HasOne(d => d.ShippingLocation)
                    .WithMany(p => p.InventoryTxes)
                    .HasForeignKey(d => d.ShippingLocationId)
                    .HasConstraintName("FK_Inventory Tx_Shipping Location");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.InventoryTxes)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Inventory Tx_Supplier");

                entity.HasOne(d => d.ToInventoryLocation)
                    .WithMany(p => p.InventoryTxToInventoryLocations)
                    .HasForeignKey(d => d.ToInventoryLocationId)
                    .HasConstraintName("FK_Inventory Tx_To Inventory Location");

                entity.HasOne(d => d.TruckingCompany)
                    .WithMany(p => p.InventoryTxes)
                    .HasForeignKey(d => d.TruckingCompanyId)
                    .HasConstraintName("FK_Inventory Tx_Trucking Company");
            });

            modelBuilder.Entity<InventoryTxDetail>(entity =>
            {
                entity.ToTable("Inventory Tx Detail");

                entity.Property(e => e.InventoryTxDetailId).HasColumnName("Inventory Tx Detail Id");

                entity.Property(e => e.FromInventoryLocationId).HasColumnName("From Inventory Location Id");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserid)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert Userid");

                entity.Property(e => e.InventoryTxId).HasColumnName("Inventory Tx Id");

                entity.Property(e => e.InventoryTxReasonId).HasColumnName("Inventory Tx Reason Id");

                entity.Property(e => e.ProductId).HasColumnName("Product Id");

                entity.Property(e => e.ToInventoryLocationId).HasColumnName("To Inventory Location Id");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserid)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update Userid");

                entity.HasOne(d => d.InventoryTx)
                    .WithMany(p => p.InventoryTxDetails)
                    .HasForeignKey(d => d.InventoryTxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory Tx Detail_Inventory Tx");

                entity.HasOne(d => d.InventoryTxReason)
                    .WithMany(p => p.InventoryTxDetails)
                    .HasForeignKey(d => d.InventoryTxReasonId)
                    .HasConstraintName("FK_Inventory Tx Detail_Inventory Tx Reason");

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

            modelBuilder.Entity<InventoryTxType>(entity =>
            {
                entity.ToTable("Inventory Tx Type");

                entity.Property(e => e.InventoryTxTypeId).HasColumnName("Inventory Tx Type Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InventoryTxTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Inventory Tx Type Code");
            });

            modelBuilder.Entity<MbllCustomer>(entity =>
            {
                entity.HasKey(e => new { e.CustomerType, e.CustomerNumber });

                entity.ToTable("MBLL Customer");

                entity.Property(e => e.CustomerType)
                    .HasMaxLength(1)
                    .HasColumnName("Customer Type")
                    .IsFixedLength();

                entity.Property(e => e.CustomerNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Customer Number");

                entity.Property(e => e.ChangeType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("Change Type")
                    .IsFixedLength();

                entity.Property(e => e.CustomerStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("Customer Status")
                    .IsFixedLength();

                entity.Property(e => e.InsertTimestamp).HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.Municipality).HasMaxLength(25);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(12)
                    .HasColumnName("Postal Code");

                entity.Property(e => e.PremiseName)
                    .HasMaxLength(40)
                    .HasColumnName("Premise Name");

                entity.Property(e => e.Province).HasMaxLength(3);

                entity.Property(e => e.SourceFile).HasMaxLength(50);

                entity.Property(e => e.StreetAddress)
                    .HasMaxLength(40)
                    .HasColumnName("Street Address");
            });

            modelBuilder.Entity<MbllSalesOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MBLL Sales Order");

                entity.Property(e => e.CreateTimestamp)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("Create Timestamp");

                entity.Property(e => e.CustomerNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Customer Number");

                entity.Property(e => e.CustomerType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("Customer Type")
                    .IsFixedLength();

                entity.Property(e => e.DeliveryDate)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("Delivery Date");

                entity.Property(e => e.DeliveryInstructions)
                    .HasMaxLength(40)
                    .HasColumnName("Delivery Instructions");

                entity.Property(e => e.DeliveryTime)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("Delivery Time");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .HasMaxLength(50)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.ItemNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Item Number");

                entity.Property(e => e.OrderType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("Order Type");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.SourceFile).HasMaxLength(50);

                entity.Property(e => e.TrxDate)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("Trx Date");

                entity.Property(e => e.TrxNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Trx Number");

                entity.Property(e => e.TrxType)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasColumnName("Trx Type");

                entity.Property(e => e.Upc)
                    .IsRequired()
                    .HasMaxLength(14)
                    .HasColumnName("UPC");
            });

            modelBuilder.Entity<OrderSource>(entity =>
            {
                entity.ToTable("Order Source");

                entity.Property(e => e.OrderSourceId).HasColumnName("Order Source Id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserId)
                    .HasMaxLength(500)
                    .HasColumnName("Update UserId");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("Product Id");

                entity.Property(e => e.CaseWeight)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Case Weight");

                entity.Property(e => e.ContainerWeight)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Container Weight");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HlCase)
                    .HasColumnType("decimal(8, 5)")
                    .HasColumnName("HL Case");

                entity.Property(e => e.HlContainer)
                    .HasColumnType("decimal(8, 5)")
                    .HasColumnName("HL Container");

                entity.Property(e => e.HlSingle)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("HL Single");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.PackSize).HasColumnName("Pack Size");

                entity.Property(e => e.SingleWeight)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Single Weight");

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SKU");

                entity.Property(e => e.SupplierId).HasColumnName("Supplier Id");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update UserId");

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

            modelBuilder.Entity<ShippingLocation>(entity =>
            {
                entity.ToTable("Shipping Location");

                entity.Property(e => e.ShippingLocationId).HasColumnName("Shipping Location Id");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserid)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert Userid");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserid)
                    .HasMaxLength(500)
                    .HasColumnName("Update Userid");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.HasIndex(e => e.SupplierCode, "IX_Supplier")
                    .IsUnique();

                entity.Property(e => e.SupplierId).HasColumnName("Supplier Id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Address2).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Contact1Name)
                    .HasMaxLength(100)
                    .HasColumnName("Contact 1 Name");

                entity.Property(e => e.Contact1Phone1)
                    .HasMaxLength(20)
                    .HasColumnName("Contact 1 Phone 1");

                entity.Property(e => e.Contact1Phone2)
                    .HasMaxLength(20)
                    .HasColumnName("Contact 1 Phone 2");

                entity.Property(e => e.Contact2Name)
                    .HasMaxLength(100)
                    .HasColumnName("Contact 2 Name");

                entity.Property(e => e.Contact2Phone1)
                    .HasMaxLength(20)
                    .HasColumnName("Contact 2 Phone 1");

                entity.Property(e => e.Contact2Phone2)
                    .HasMaxLength(20)
                    .HasColumnName("Contact 2 Phone 2");

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.GeneralPhone)
                    .HasMaxLength(20)
                    .HasColumnName("General Phone");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert UserId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(12)
                    .HasColumnName("Postal Code");

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.SupplierCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("Supplier Code");

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Update UserId");
            });

            modelBuilder.Entity<Territory>(entity =>
            {
                entity.ToTable("Territory");

                entity.Property(e => e.TerritoryId).HasColumnName("Territory Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TruckingCompany>(entity =>
            {
                entity.ToTable("Trucking Company");

                entity.Property(e => e.TruckingCompanyId).HasColumnName("Trucking Company Id");

                entity.Property(e => e.InsertTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Insert Timestamp");

                entity.Property(e => e.InsertUserid)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Insert Userid");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTimestamp)
                    .HasPrecision(0)
                    .HasColumnName("Update Timestamp");

                entity.Property(e => e.UpdateUserid)
                    .HasMaxLength(500)
                    .HasColumnName("Update Userid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
