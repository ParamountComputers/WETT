using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class FulfillSalesViewModel
    {
        public long CustomerOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DelveryDate { get; set; }
        public string OrderNumber { get; set; }
        public string Customer { get; set; }
        public string City { get; set; }
        public long CarrierID { get; set; }
        public string CarrierDesc { get; set; }
        public string Instructions { get; set; }
        public string Status { get; set; }
        public string ShortInv { get; set; }

        //public List<SpGetFulfillSalesHdr> FulfillSalesHdrs { get; set; }
    }
}

