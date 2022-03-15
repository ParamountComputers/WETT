using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class InventoryTx
    {
        public InventoryTx()
        {
            InventoryTxDetails = new HashSet<InventoryTxDetail>();
        }

        public long InventoryTxId { get; set; }
        public DateTime Date { get; set; }
        public long InventoryTxReasonId { get; set; }
        public string Comments { get; set; }

        public virtual InventoryTxReason InventoryTxReason { get; set; }
        public virtual ICollection<InventoryTxDetail> InventoryTxDetails { get; set; }
    }
}
