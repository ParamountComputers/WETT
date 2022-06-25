using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class InventoryLocation
    {
        public InventoryLocation()
        {
            Inventories = new HashSet<Inventory>();
            InventoryTxFromInventoryLocations = new HashSet<InventoryTx>();
            InventoryTxToInventoryLocations = new HashSet<InventoryTx>();
        }

        public long InventoryLocationId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryTx> InventoryTxFromInventoryLocations { get; set; }
        public virtual ICollection<InventoryTx> InventoryTxToInventoryLocations { get; set; }
    }
}
