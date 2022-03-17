using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class SupplierOrderDetail
    {
        public long SupplierOrderDetailId { get; set; }
        public long SupplierOrderId { get; set; }
        public long ProductId { get; set; }
        public long InventoryLocationId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual SupplierOrder SupplierOrder { get; set; }
    }
}
