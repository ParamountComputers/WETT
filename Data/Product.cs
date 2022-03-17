using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class Product
    {
        public Product()
        {
            CustomerOrderDetails = new HashSet<CustomerOrderDetail>();
            InventoryTxDetails = new HashSet<InventoryTxDetail>();
            SupplierOrderDetails = new HashSet<SupplierOrderDetail>();
        }

        public long ProductId { get; set; }
        public long SupplierId { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal Weight { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateTimestamp { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual ICollection<CustomerOrderDetail> CustomerOrderDetails { get; set; }
        public virtual ICollection<InventoryTxDetail> InventoryTxDetails { get; set; }
        public virtual ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; }
    }
}
