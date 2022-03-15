using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class CustomerOrderDetail
    {
        public long CustomerOrderDetailId { get; set; }
        public long CustomerOrderId { get; set; }
        public long ProductId { get; set; }
        public int QtyOrdered { get; set; }
        public int? QtyFulfilled { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
