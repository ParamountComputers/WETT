using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class CustomerOrderStatus
    {
        public CustomerOrderStatus()
        {
            CustomerOrders = new HashSet<CustomerOrder>();
        }

        public long CustomerOrderStatusId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
    }
}
