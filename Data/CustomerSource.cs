using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class CustomerSource
    {
        public CustomerSource()
        {
            Customers = new HashSet<Customer>();
        }

        public long CustomerSourceId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
