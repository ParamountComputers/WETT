using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class CustomerLob
{
    public long CustomerId { get; set; }

    public string LobCode { get; set; }

    public virtual Customer Customer { get; set; }
}
