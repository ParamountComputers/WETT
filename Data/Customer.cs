﻿using System;
using System.Collections.Generic;

namespace WETT
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerOrders = new HashSet<CustomerOrder>();
        }

        public long CustomerId { get; set; }
        public long CustomerTypeId { get; set; }
        public long CustomerSourceId { get; set; }
        public long? CallFrequencyId { get; set; }
        public long? TerritoryId { get; set; }
        public long? SegmentId { get; set; }
        public long? CdosId { get; set; }
        public decimal? MbllCustomerNo { get; set; }
        public string LicenceNumber { get; set; }
        public long? CustomerStatusId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; }
        public string Phone1Type { get; set; }
        public string Phone1 { get; set; }
        public string Phone2Type { get; set; }
        public string Phone2 { get; set; }
        public string Phone3Type { get; set; }
        public string Phone3 { get; set; }
        public string ContactEmail { get; set; }
        public bool DeletedFlag { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string InsertUserId { get; set; }
        public DateTime? InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime? UpdateTimestamp { get; set; }

        public virtual CallFrequency CallFrequency { get; set; }
        public virtual Cdo Cdos { get; set; }
        public virtual CustomerSource CustomerSource { get; set; }
        public virtual CustomerType CustomerType { get; set; }
        public virtual Segment Segment { get; set; }
        public virtual Territory Territory { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
    }
}
