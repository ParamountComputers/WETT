using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WETT.Data;
using WETT.Models;

namespace WETT.Controllers
{
    public class invAdjController : Controller
    {
        private readonly WETT_DBContext _context;
        public invAdjController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.InventoryTxes
                .Include(pi => pi.InventoryTxDetails)
                .ThenInclude(pi => pi.Product)
                .ThenInclude(pi => pi.Supplier)
                .AsNoTracking()
                .ToListAsync());
        }

        public JsonResult GetAll(JqGridViewModel request)
        {
            var wETT_DBContext = _context.Suppliers;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var invAdjData = _context.InventoryTxes
                .Include(pi => pi.InventoryTxDetails);



            //bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            //if (issearch)
            //    foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
            //    {
            //        switch (rule.field)
            //        {
            //            case "name":
            //                invAdjData = invAdjData.Where(w => w.InventoryTxDetails.Contains(rule.data)).ToList();
            //                break;
            //        }
            //    }

            int totalRecords = invAdjData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            ////Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            //if (request.sord.ToUpper() == "DESC")
            //{
            //    invAdjData = invAdjData.OrderByDescending(t => t.InventoryTxDetails.ProductId).ToList();
            //    invAdjData = invAdjData.Skip(currentPageIndex * request.rows).Take(request.rows).ToList();
            //}
            //else
            //{
            //    invAdjData = invAdjData.OrderBy(t => t.InventoryTxDetails.ProductId).ToList();
            //    invAdjData = invAdjData.Skip(currentPageIndex * request.rows).Take(request.rows).ToList();
            //}

            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = invAdjData
            };

            return Json(jsonData);
        }

    }
}
