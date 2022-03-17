using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class InventoryLocation
    {
        public InventoryLocation()
        {
            Inventories = new HashSet<Inventory>();
            InventoryTxDetails = new HashSet<InventoryTxDetail>();
        }

        public long InventoryLocationId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryTxDetail> InventoryTxDetails { get; set; }
    }
}
