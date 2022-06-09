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

            var result = from b in _context.InventoryTxDetails
                         join a in _context.InventoryTx on b.InventoryTxId equals a.InventoryTxId
                         join c in _context.InventoryTxReasons on a.InventoryTxReasonId equals c.InventoryTxReasonId
                         join d in _context.InventoryLocations on b.InventoryLocationId equals d.InventoryLocationId
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         select new invAdjViewModel
                         {
                             InventoryTxId = a.InventoryTxId,
                             ProductId = e.ProductId,
                             SupplierName = f.Name,
                             ProductName = e.Description,
                             InventoryLocation = d.Description,
                             Amount = b.Amount,
                             InventoryTxReason = c.Description,
                             Comments = a.Comments,
                             Date = a.Date, //.ToShortDateString(),
                             SaCode = "1s2s3"
                                 
                             };
            return View(result);
        }

        public JsonResult GetAll(JqGridViewModel request)
        {
            var wETT_DBContext = _context.Suppliers;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var invAdjData = from b in _context.InventoryTxDetails
                             join a in _context.InventoryTx on b.InventoryTxId equals a.InventoryTxId
                             join c in _context.InventoryTxReasons on a.InventoryTxReasonId equals c.InventoryTxReasonId
                             join d in _context.InventoryLocations on b.InventoryLocationId equals d.InventoryLocationId
                             join e in _context.Products on b.ProductId equals e.ProductId
                             join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                             select new invAdjViewModel
                             {
                                 InventoryTxId= a.InventoryTxId,
                                 ProductId = e.ProductId,
                                 SupplierName = f.Name,
                                 ProductName = e.Description,
                                 InventoryLocation = d.Description,
                                 Amount = b.Amount,
                                 InventoryTxReason = c.Description,
                                 Comments = a.Comments,
                                 Date = a.Date,
                                 SaCode = "1s2s3"
                             };



            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "date":
                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Date.Equals(DateTime.Parse(rule.data)));
                            break;
                        case "productName":
                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.ProductName.Contains(rule.data));
                            break;
                        case "saCode":
                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.SaCode.Contains(rule.data));
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
        public IActionResult CreateList()
        {

            var li = from s in _context.Suppliers.Where(a => a.ActiveFlag == "Y")
                     select new
                     {
                         text = s.Name,
                         
                     };
            return Json(li);
        }
        public IActionResult CreateProductIdList()
        {
            var invAdjData = from a in _context.Products
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             select new
                     {
                         text = a.ProductId,
                         value = b.Name

                     };
            return Json(invAdjData);
        }


      }
    }
