using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class InventoryTxReason
    {
        public InventoryTxReason()
        {
            InventoryTxDetails = new HashSet<InventoryTxDetail>();
        }

        public long InventoryTxReasonId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<InventoryTxDetail> InventoryTxDetails { get; set; }
    }
}
