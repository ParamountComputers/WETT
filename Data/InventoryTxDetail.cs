using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class InventoryTxDetail
    {
        public long InventoryTxDetailId { get; set; }
        public long InventoryTxId { get; set; }
        public long ProductId { get; set; }
        public long? FromInventoryLocationId { get; set; }
        public long? ToInventoryLocationId { get; set; }
        public long? InventoryTxReasonId { get; set; }
        public int Amount { get; set; }
        public string InsertUserid { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserid { get; set; }
        public DateTime UpdateTimestamp { get; set; }

        public virtual InventoryTx InventoryTx { get; set; }
        public virtual InventoryTxReason InventoryTxReason { get; set; }
        public virtual Product Product { get; set; }
    }
}
