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
    public class SaOutboundController : Controller
    {
        private DateTime searchDate;
        private readonly WETT_DBContext _context;
        public SaOutboundController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            var result = from b in _context.InventoryTxDetails
                         join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                         join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                         join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         select new SaOutboundViewModel
                         {
                             SealNo = "1ww1",
                             TruckingCompany = "ken",
                             TruckerProbillNumber = 123,
                             PurchaseOrder = "1ss2",
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = e.Sku,
                             SupplierName = f.Name,
                             ProductId = e.ProductId,
                             ProductName = e.Description,
                             InventoryLocationId = d.InventoryLocationId,
                             InventoryLocation = d.Description,
                             Amount = b.Amount,
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
            var SaOutboundData = from b in _context.InventoryTxDetails
                                      join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                      join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                      join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                                      join e in _context.Products on b.ProductId equals e.ProductId
                                      join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                      select new SaOutboundViewModel
                                      {
                                         SealNo = "1ww1",
                                         TruckingCompany = "ken",
                                         TruckerProbillNumber = 123,
                                         PurchaseOrder = "1ss2",
                                         InventoryTxDetailId = b.InventoryTxDetailId,
                                         ProductSku = e.Sku,
                                         SupplierName = f.Name,
                                         ProductId = e.ProductId,
                                         ProductName = e.Description,
                                         InventoryLocationId = d.InventoryLocationId,
                                          InventoryLocation = d.Description,
                                          Amount = b.Amount,
                                         Comments = a.Comments,
                                         Date = a.Date, //.ToShortDateString(),
                                         SaCode = "1s2s3"

                                     };



            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "date":
                            SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.Date.Equals(DateTime.Parse(rule.data)));
                            searchDate = DateTime.Parse(rule.data);
                            break;
                        case "productName":
                            SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.ProductName.Contains(rule.data));
                            break;
                        case "saCode":
                            SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.SaCode.Contains(rule.data));
                            break;
                        case "comments":
                            SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.Comments.Contains(rule.data));
                            break;
                        case "locationsDropdown":
                            if (rule.data.Contains("-1"))
                            {
                               

                                break; 
                            }
                            else
                            {
                                SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.InventoryLocationId.ToString().Contains(rule.data));
                                break;
                            }
                        case "purchaseOrder":
                            SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.PurchaseOrder.Contains(rule.data));
                            break;

                    }
                }

            int totalRecords = SaOutboundData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.OrderByDescending(t => t.ProductName);
                SaOutboundData = SaOutboundData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.OrderBy(t => t.ProductName);
                SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = SaOutboundData
            };

            return Json(jsonData);
        }
        public JsonResult Add(SaOutboundViewModel p)
        {
            InventoryTx s = new InventoryTx
            {
                Date = searchDate,
                Comments = p.Comments,

            };
            _context.InventoryTxes.Add(s);
            _context.SaveChanges();

            InventoryTxDetail r = new InventoryTxDetail
            {
                InventoryTxId=s.InventoryTxId,
                ToInventoryLocationId = p.InventoryLocationId,
                ProductId = p.ProductId,
                Amount = p.Amount
            };
            _context.InventoryTxDetails.Add(r);
            _context.SaveChanges();
            return Json(true);

        }
        public JsonResult Update(SaOutboundViewModel p)
        {


            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.ToInventoryLocationId = p.InventoryLocationId;
            r.ProductId = p.ProductId;
            r.Amount = p.Amount;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Delete(long id)
        {
            InventoryTxDetail r = _context.InventoryTxDetails.Single(e => e.InventoryTxDetailId == id);
            _context.InventoryTxDetails.Remove(r);
            _context.SaveChanges();


            return Json(true);
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
        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from a in _context.Products
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             select new
                             {
                                 text = a.Sku,
                                 value = b.Name

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductName()
        {
            var invAdjData = from a in _context.Products
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             select new
                             {
                                 text = a.Description,
                                 value = b.Name,
                                 id = a.ProductId

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateLocationsList()
        {
            var invAdjData = from a in _context.InventoryLocations
                             select new
                             {
                                 value = a.InventoryLocationId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
    }
}
