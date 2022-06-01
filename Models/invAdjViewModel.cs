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
        public IEnumerable<InventoryTx> InventoryHeader { get; set; }
        public IEnumerable<InventoryTxDetail> InventoryBody { get; set; }

        internal Task<string> ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
