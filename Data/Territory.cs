using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class Territory
    {
        public Territory()
        {
            Customers = new HashSet<Customer>();
        }

        public long TerritoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
