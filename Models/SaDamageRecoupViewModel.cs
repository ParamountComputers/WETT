using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class SaDamageRecoupViewModel
    {
        public long InventoryTxId { get; set; }
        public string ProductCode { get; set; }

        public string SupplierName { get; set; }

        public string ProductName { get; set; }

        public string InventoryLocation { get; set; }

        public int Amount { get; set; }

        public string InventoryTxReason { get; set; }

        public string Comments { get; set; }

        public DateTime Date { get; set; }

        public string SaCode { get; set; }
    }
}
