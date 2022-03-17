using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class SupplierOrder
    {
        public SupplierOrder()
        {
            SupplierOrderDetails = new HashSet<SupplierOrderDetail>();
        }

        public long SupplierOrderId { get; set; }
        public long SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateTimestamp { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; }
    }
}
