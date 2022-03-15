using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class CustomerType
    {
        public CustomerType()
        {
            Customers = new HashSet<Customer>();
        }

        public long CustomerTypeId { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
