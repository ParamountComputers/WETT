using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class invAdjViewModel 
    {
        public long ProductId { get; set; }

        public string SupplierName { get; set; }

        public string ProductName { get; set; }

        public string InventoryLocation { get; set; }

        public int Amount { get; set; }

        public string InventoryTxReason { get; set; }

        public string Comments { get; set; }

        public string Date { get; set; }
    }
}
