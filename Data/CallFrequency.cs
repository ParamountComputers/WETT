using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class CallFrequency
    {
        public CallFrequency()
        {
            Customers = new HashSet<Customer>();
        }

        public long CallFrequencyId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
