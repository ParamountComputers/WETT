using System;

namespace WETT.Models
{
    public class CustomerSummaryViewModel
    {
        public long CustomerOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DelveryDate { get; set; }
        public string OrderNumber { get; set; }
        public string Customer { get; set; }
        public string City { get; set; }
        public string Carrier { get; set; }
        public long CarrierId { get; set; }
        public string CarrierDesc { get; set; }
        public string Instructions { get; set; }
        public string Status { get; set; }

    }
}
