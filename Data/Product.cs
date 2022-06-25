using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class Product
    {
        public Product()
        {
            CustomerOrderDetails = new HashSet<CustomerOrderDetail>();
            Inventories = new HashSet<Inventory>();
            InventoryTxDetails = new HashSet<InventoryTxDetail>();
        }

        public long ProductId { get; set; }
        public long SupplierId { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal SingleWeight { get; set; }
        public decimal ContainerWeight { get; set; }
        public decimal CaseWeight { get; set; }
        public int PackSize { get; set; }
        public string HlSingle { get; set; }
        public decimal HlContainer { get; set; }
        public decimal HlCase { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateTimestamp { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<CustomerOrderDetail> CustomerOrderDetails { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryTxDetail> InventoryTxDetails { get; set; }
    }
}
