﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class invAdjViewModel 
    {
        public long InventoryTxDetailId { get; set; }
        public string ProductSku { get; set; }

        public string SupplierName { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }
        public long InventoryLocationId { get; set; }

        public int Amount { get; set; }

        public long InventoryTxReasonId { get; set; }

        public string Comments { get; set; }

        public DateTime Date { get; set; }

        public string SaCode { get; set; }
    }
}