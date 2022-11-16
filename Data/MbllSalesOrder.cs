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
        public string CustomerNumber { get; set; }
        public string TrxDate { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public string DeliveryInstructions { get; set; }
        public string ItemNumber { get; set; }
        public string Quantity { get; set; }
        public string CreateTimestamp { get; set; }
        public string Upc { get; set; }
        public string SourceFile { get; set; }
        public string InsertUserId { get; set; }
        public DateTime? InsertTimestamp { get; set; }
    }
}
