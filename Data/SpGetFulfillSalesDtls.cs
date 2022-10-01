using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WETT.Data

{
    public class SpGetFulfillSalesDtls
    {
        public long CustomerOrderDtlsID { get; set; }
        public long CustomerOrderID { get; set; }
        public long ProductID { get; set; }
        public string ProductSku { get; set; }
        public string ProductDesc { get; set; }
        public int StockQty { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyFulfilled { get; set; }

        public string Notes { get; set; }


    }
}
