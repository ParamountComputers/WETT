using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class InventoryTxType
    {
        public InventoryTxType()
        {
            InventoryTxes = new HashSet<InventoryTx>();
        }

        public long InventoryTxTypeId { get; set; }
        public string InventoryTxTypeCode { get; set; }
        public string Description { get; set; }

        public virtual ICollection<InventoryTx> InventoryTxes { get; set; }
    }
}
