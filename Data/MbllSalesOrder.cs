using System;
using System.Collections.Generic;

namespace WETT.Data
{
    public partial class MbllSalesOrder
    {
        public string TrxNumber { get; set; }
        public string TrxType { get; set; }
        public string OrderType { get; set; }
        public string CustomerType { get; set; }
        public decimal CustomerNumber { get; set; }
        public decimal TrxDate { get; set; }
        public decimal DeliveryDate { get; set; }
        public decimal DeliveryTime { get; set; }
        public string DeliveryInstructions { get; set; }
        public decimal ItemNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal CreateTimestamp { get; set; }
        public string Upc { get; set; }
        public string InsertUserId { get; set; }
        public DateTime? InsertTimestamp { get; set; }
    }
}
