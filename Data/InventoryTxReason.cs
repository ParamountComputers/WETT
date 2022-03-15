using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class InventoryTxReason
    {
        public InventoryTxReason()
        {
            InventoryTxes = new HashSet<InventoryTx>();
        }

        public long InventoryTxReasonId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<InventoryTx> InventoryTxes { get; set; }
    }
}
