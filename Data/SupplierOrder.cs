using System;
using System.Collections.Generic;

namespace WETT
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

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; }
    }
}
