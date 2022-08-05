using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class OrderSource
    {
        public OrderSource()
        {
            CustomerOrders = new HashSet<CustomerOrder>();
        }

        public long OrderSourceId { get; set; }
        public string Description { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime? UpdateTimestamp { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
    }
}
