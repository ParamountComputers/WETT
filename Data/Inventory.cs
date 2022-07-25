using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class Inventory
    {
        public long ProductId { get; set; }
        public long InventoryLocationId { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        //public string InsertUserId { get; set; }

        public virtual InventoryLocation InventoryLocation { get; set; }
        public virtual Product Product { get; set; }
    }
}
