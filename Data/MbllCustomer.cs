using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class MbllCustomer
    {
        public string CustomerType { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerStatus { get; set; }
        public string ChangeType { get; set; }
        public string PremiseName { get; set; }
        public string StreetAddress { get; set; }
        public string Municipality { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        //public string InsertUserId { get; set; }
        //public DateTime? InsertTimestamp { get; set; }
    }
}
