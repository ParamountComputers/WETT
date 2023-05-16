using System;

namespace WETT.Models
{
    public class CanCustSummViewModel
    {
        public long CustomerOrderID { get; set; }
        public long CustomerID { get; set; }
        public long SupplierID { get; set; }
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
        public long OrderStatusId { get; set; }
        public string LOBCode { get; set; }

    }
}
