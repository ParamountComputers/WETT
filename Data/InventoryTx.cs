using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class InventoryTx
    {
        public InventoryTx()
        {
            InventoryTxDetails = new HashSet<InventoryTxDetail>();
        }
        public string SaCode { get; set; }
        public long InventoryTxId { get; set; }
        public DateTime Date { get; set; }
        public long InventoryTxReasonId { get; set; }
        public string Comments { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateTimestamp { get; set; }

        public virtual InventoryTxReason InventoryTxReason { get; set; }
        public virtual ICollection<InventoryTxDetail> InventoryTxDetails { get; set; }
    }
}
