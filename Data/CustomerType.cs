using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class CustomerType
    {
        public CustomerType()
        {
            Customers = new HashSet<Customer>();
        }

        public string CustomerTypeCode { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
