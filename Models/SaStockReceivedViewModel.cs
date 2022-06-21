using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class SaStockReceivedViewModel
    {
        public string SealNo { get; set; }
        public string TruckingCompany { get; set; }
        public long TruckerProbillNumber { get; set; }
        
        public string PurchaseOrder { get; set; }
        public long InventoryTxDetailId { get; set; }
        public string ProductSku { get; set; }

        public string SupplierName { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }
        public long InventoryLocationId { get; set; }
        public string InventoryLocation { get; set; }

        public int Amount { get; set; }

        public string Comments { get; set; }

        public DateTime Date { get; set; }

        public string SaCode { get; set; }
    }
}
