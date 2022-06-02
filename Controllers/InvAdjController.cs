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
            //var table = from inv in _context.InventoryTxes
            //            join invR in _context.InventoryTxReasons on inv.InventoryTxReasonId equals invR.InventoryTxReasonId
            //            join invD in _context.InventoryTxDetails on inv.InventoryTxId equals invD.InventoryTxId
            //            join invL in _context.InventoryLocations on invD.InventoryLocationId equals invL.InventoryLocationId
            //            join prod in _context.Products on invD.ProductId equals prod.ProductId
            //            join supp in _context.Suppliers on prod.SupplierId equals supp.SupplierId
            //            select inv;
            //return View(table);
            var result = from a in _context.InventoryTx
                         join b in _context.InventoryTxDetails on a.InventoryTxId equals b.InventoryTxId
                         join c in _context.InventoryTxReasons on a.InventoryTxReasonId equals c.InventoryTxReasonId
                         join d in _context.InventoryLocations on b.InventoryLocationId equals d.InventoryLocationId
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         select new invAdjViewModel
                         {
                             ProductId = e.ProductId,
                             SupplierName = f.Name,
                             ProductName = e.Description,
                             InventoryLocation = d.Description,
                             Amount = b.Amount,
                             InventoryTxReason= c.Description,
                             Comments= a.Comments
                         };
            return View(result);
        }

        public JsonResult GetAll(JqGridViewModel request)
        {
            var wETT_DBContext = _context.Suppliers;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var invAdjData = from a in _context.InventoryTx
                         join b in _context.InventoryTxDetails on a.InventoryTxId equals b.InventoryTxId
                         join c in _context.InventoryTxReasons on a.InventoryTxReasonId equals c.InventoryTxReasonId
                         join d in _context.InventoryLocations on b.InventoryLocationId equals d.InventoryLocationId
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         select new invAdjViewModel
                         {
                             ProductId = e.ProductId,
                             SupplierName = f.Name,
                             ProductName = e.Description,
                             InventoryLocation = d.Description,
                             Amount = b.Amount,
                             InventoryTxReason = c.Description,
                             Comments = a.Comments,
                             Date=a.Date.ToShortDateString()
                         };



            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "date":
                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Date.Contains(rule.data));
                            break;
                        case "productName":
                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.ProductName.Contains(rule.data));
                            break;
                        case "saCode":
                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.ProductName.Contains(rule.data));
                            break;
                    }
                }

            int totalRecords = invAdjData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.OrderByDescending(t => t.ProductName);
                invAdjData = invAdjData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.OrderBy(t => t.ProductName);
                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
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
