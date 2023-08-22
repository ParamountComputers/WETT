using Microsoft.AspNetCore.Mvc;

namespace WETT.Models
{
    public class CustomerOrderViewModel
    {

        public long CustomerOrderDetailId { get; set; }
        public long CustomerOrderID { get; set; }
        public string OrderNumber { get; set; }
        public long ProductID { get; set; }
        public string ProductSku { get; set; }
        public string ProductDesc { get; set; }
        public long StockQty { get; set; }
        public int QtyOrdered { get; set; }
        public int? QtyFulfilled { get; set; }
        public string Notes { get; set; }

    }
}
