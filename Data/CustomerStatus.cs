using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class CustomerStatus
{
    public string CustomerStatusCode { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}
