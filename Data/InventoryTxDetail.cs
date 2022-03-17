using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class InventoryTxDetail
    {
        public long InventoryTxDetailId { get; set; }
        public long InventoryTxId { get; set; }
        public long ProductId { get; set; }
        public long InventoryLocationId { get; set; }
        public int Amount { get; set; }

        public virtual InventoryLocation InventoryLocation { get; set; }
        public virtual InventoryTx InventoryTx { get; set; }
        public virtual Product Product { get; set; }
    }
}
