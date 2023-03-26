using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class TruckingCompany
{
    public long TruckingCompanyId { get; set; }

    public string Name { get; set; }

    public string InsertUserid { get; set; }

    public DateTime InsertTimestamp { get; set; }

    public string UpdateUserid { get; set; }

    public DateTime? UpdateTimestamp { get; set; }

    public virtual ICollection<InventoryTx> InventoryTxes { get; } = new List<InventoryTx>();
}
