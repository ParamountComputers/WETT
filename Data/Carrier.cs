using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class Carrier
{
    public long CarrierId { get; set; }

    public string Name { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string City { get; set; }

    public string Province { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string ContactName { get; set; }

    public string ContactPhone { get; set; }

    public string InsertUserId { get; set; }

    public DateTime InsertTimestamp { get; set; }

    public string UpdateUserId { get; set; }

    public DateTime UpdateTimestamp { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; } = new List<CustomerOrder>();
}
