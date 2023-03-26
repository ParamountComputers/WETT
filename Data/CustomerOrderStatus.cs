using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class CustomerOrderStatus
{
    public long CustomerOrderStatusId { get; set; }

    public string Description { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; } = new List<CustomerOrder>();
}
