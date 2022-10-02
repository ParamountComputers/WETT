using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class FulfillSalesDtlsViewModel
    {
        
        public long CustomerOrderDtlsID { get; set; }
        public long CustomerOrderID { get; set; }
        public long ProductID { get; set; }
        public string ProductSku { get; set; }
        public string ProductDesc { get; set; }
        public long StockQty { get; set; }
        public int QtyPending { get; set; }
        public int QtyOrdered { get; set; }
        public int? QtyFulfilled { get; set; }
        public string Notes { get; set; }

    }
}
