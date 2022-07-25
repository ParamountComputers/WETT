using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class InvTxSummaryViewModel
    {

        public long InventoryTxTypeId { get; set; }

        public string Comments { get; set; }

        public DateTime Date { get; set; }

        public string SaCode { get; set; }
        public long InventoryTxId { get; set; }
    }
}