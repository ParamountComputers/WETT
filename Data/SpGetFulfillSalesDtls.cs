using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WETT.Data

{
    public class SpGetFulfillSalesDtls
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

    }
}
