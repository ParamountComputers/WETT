using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class InventoryTxDetail
    {
        public long InventoryTxDetailId { get; set; }
        public long InventoryTxId { get; set; }
        public long ProductId { get; set; }
        public long InventoryLocationId { get; set; }
        public int Amount { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public string UpdateTimestamp { get; set; }

        public virtual InventoryLocation InventoryLocation { get; set; }
        public virtual InventoryTx InventoryTx { get; set; }
        public virtual Product Product { get; set; }
    }
}
