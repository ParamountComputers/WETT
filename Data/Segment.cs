using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class Segment
    {
        public Segment()
        {
            Customers = new HashSet<Customer>();
        }

        public long SegmentId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
