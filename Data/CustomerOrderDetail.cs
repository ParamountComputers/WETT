using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class CustomerOrderDetail
    {
        public long CustomerOrderDetailId { get; set; }
        public long CustomerOrderId { get; set; }
        public long ProductId { get; set; }
        public int QtyOrdered { get; set; }
        public int? QtyFulfilled { get; set; }
        public string Notes { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserid { get; set; }
        public string UpdateTimestamkp { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
