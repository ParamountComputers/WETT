using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class Cdo
    {
        public Cdo()
        {
            Customers = new HashSet<Customer>();
        }

        public long CdosId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
