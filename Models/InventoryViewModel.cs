using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using WETT.Data;

namespace WETT.Models
{
    public class InventoryViewModel
    {

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSku { get; set; }

        public string Supplier { get; set; }

        public long InvLocationId { get; set; }

        public string InvLocationName { get; set; }

        public long InvCount { get; set; }

        public DateTime Date { get; set; }

        public DateTime SysDate { get; set; }

    }
}