﻿using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
            SupplierOrders = new HashSet<SupplierOrder>();
        }

        public long SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string GeneralPhone { get; set; }
        public string Contact1Name { get; set; }
        public string Contact1Phone1 { get; set; }
        public string Contact1Phone2 { get; set; }
        public string Contact2Name { get; set; }
        public string Contact2Phone1 { get; set; }
        public string Contact2Phone2 { get; set; }
        public string InsertUserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateTimestamp { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SupplierOrder> SupplierOrders { get; set; }
    }
}
