using System;
using System.Collections.Generic;

namespace WETT.Data;

public partial class InventoryLocation
{
    public long InventoryLocationId { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Inventory> Inventories { get; } = new List<Inventory>();

    public virtual ICollection<InventoryTx> InventoryTxFromInventoryLocations { get; } = new List<InventoryTx>();

    public virtual ICollection<InventoryTx> InventoryTxToInventoryLocations { get; } = new List<InventoryTx>();
}
