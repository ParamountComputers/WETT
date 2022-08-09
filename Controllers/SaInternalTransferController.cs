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
    public class SaInternalTransferController : Controller
    {

        public static Boolean showPage = false;
        public static string searchDate = DateTime.Today.ToShortDateString();
        public static string Notes;
        public static long CurrentHeaderId;
        public static string CurrentSaCode;
        public static long InventoryTxCurrentId;
        public static long CurrentToLocation;
        public static long CurrentFromLocation;
        private readonly WETT_DBContext _context;
        public SaInternalTransferController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string SaCode)
        {
            CurrentSaCode = SaCode;
            InventoryTxCurrentId = -1;
            var result = from b in _context.InventoryTxDetails
                         join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                         join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                        // join d in _context.InventoryLocations on a.ToInventoryLocationId equals d.InventoryLocationId
                        // join d2 in _context.InventoryLocations on a.FromInventoryLocationId equals d2.InventoryLocationId
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                         where c.InventoryTxTypeId == 6
                         select new SaInternalTransferViewModel
                         {
                             InventoryTxId = b.InventoryTxId,
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = e.Sku,
                             SupplierName = f.Name,
                             ProductId = e.ProductId,
                             ProductName = e.Description,
                             toInventoryLocationId = (long)a.ToInventoryLocationId,
                             fromInventoryLocationId = (long)a.FromInventoryLocationId,
                             InventoryTxReasonsId = g.InventoryTxReasonId,
                             Amount = b.Amount,
                             InventoryTxTypeId = c.InventoryTxTypeId,
                             Comments = b.Comments,
                             Date = a.Date, //.ToShortDateString(),
                             SaCode = a.StockAdjCode

                         };
            return View(result);
        }

        public JsonResult GetAll(JqGridViewModel request)
        {

            var AllSaInternalTransferData = from b in _context.InventoryTxDetails
                                   join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                   join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                   join d in _context.InventoryLocations on a.ToInventoryLocationId equals d.InventoryLocationId
                                   join d2 in _context.InventoryLocations on a.FromInventoryLocationId equals d2.InventoryLocationId
                                   join e in _context.Products on b.ProductId equals e.ProductId
                                   join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                   join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                                   where c.InventoryTxTypeId == 6
                                   select new SaInternalTransferViewModel
                                   {
                                       InventoryTxId = b.InventoryTxId,
                                       InventoryTxDetailId = b.InventoryTxDetailId,
                                       ProductSku = e.Sku,
                                       SupplierName = f.Name,
                                       ProductId = e.ProductId,
                                       ProductName = e.Description,
                                       toInventoryLocationId = d.InventoryLocationId,
                                       fromInventoryLocationId = d2.InventoryLocationId,
                                       InventoryTxReasonsId = g.InventoryTxReasonId,
                                       Amount = b.Amount,
                                       InventoryTxTypeId = c.InventoryTxTypeId,
                                       Comments = b.Comments,
                                       Date = a.Date, //.ToShortDateString(),
                                       SaCode = a.StockAdjCode
                                   };
            var SaInternalTransferData = AllSaInternalTransferData;



            //bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            //if (issearch)
            //    foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
            //    {
            //        switch (rule.field)
            //        {
            //            case "date":

            //                SaInternalTransferData = (IQueryable<SaInternalTransferViewModel>)SaInternalTransferData.Where(w => w.Date.Equals(DateTime.Parse(rule.data)));
            //                searchDate = rule.data;
            //                break;
            //            case "comments":

            //                SaInternalTransferData = (IQueryable<SaInternalTransferViewModel>)SaInternalTransferData.Where(w => w.Comments.Contains(rule.data));
            //                SaInternalTransferData = (IQueryable<SaInternalTransferViewModel>)SaInternalTransferData.Where(w => w.Date.Equals(DateTime.Parse(searchDate)));


            //                Notes = rule.data;
            //                break;
            //        }
            //    }
            if (CurrentSaCode != null)
            {
                SaInternalTransferData = SaInternalTransferData.Where(w => w.SaCode == CurrentSaCode);
            }
            else
            {
                if (showPage == true)
                {
                    //this is the type of transaction id
                   // SaInternalTransferData = SaInternalTransferData.Where(w => w.InventoryTxTypeId == 6);
                    SaInternalTransferData = SaInternalTransferData.Where(w => w.InventoryTxId == InventoryTxCurrentId);

                }
                else
                {
                    //this is to hide all transactons by type
                    SaInternalTransferData = SaInternalTransferData.Where(w => w.InventoryTxId == -1);
                }
            }


            int totalRecords = SaInternalTransferData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                SaInternalTransferData = (IQueryable<SaInternalTransferViewModel>)SaInternalTransferData.OrderByDescending(t => t.ProductName);
                SaInternalTransferData = SaInternalTransferData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                SaInternalTransferData = (IQueryable<SaInternalTransferViewModel>)SaInternalTransferData.OrderBy(t => t.ProductName);
                SaInternalTransferData = (IQueryable<SaInternalTransferViewModel>)SaInternalTransferData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = SaInternalTransferData
            };

            return Json(jsonData);
        }
        public JsonResult Update(SaInternalTransferViewModel p)
        {
            Product s = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.ToInventoryLocationId = CurrentToLocation;
            r.FromInventoryLocationId = CurrentFromLocation;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            r.Comments = p.Comments;
            r.InventoryTxReasonId = p.InventoryTxReasonsId;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(SaInternalTransferViewModel p)
        {
            showPage = true;
            Product s = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = new InventoryTxDetail
            {
                Comments = p.Comments,
                ToInventoryLocationId = CurrentToLocation,
                FromInventoryLocationId= CurrentFromLocation,
                ProductId = s.ProductId,
                Amount = p.Amount,
                InventoryTxReasonId = p.InventoryTxReasonsId,
                InventoryTxId = CurrentHeaderId
            };

            _context.InventoryTxDetails.Add(r);
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
        public IActionResult CreateHeader(string data)
        {
            var li = data.Split("/");
            InventoryTx s = new InventoryTx
            {
                Date = DateTime.Parse(li[0]),
                InventoryTxTypeId = 6,          //hard coded transaction type id for now
                StockAdjCode = "IT",
                //add in extra cols here******************************************            
                ToInventoryLocationId = (long)Convert.ToDouble(li[1]),
                FromInventoryLocationId = (long)Convert.ToDouble(li[2]),
                Comments = li[3]
                //*****************************************************************
            };
            //+date+'/'+ toLocationsDropdown+'/'+fromLocationsDropdown+'/'+ notes, function(data) {
            _context.InventoryTxes.Add(s);
            _context.SaveChanges();
            InventoryTxCurrentId = s.InventoryTxId;
            s.StockAdjCode = s.StockAdjCode + s.InventoryTxId;
            _context.SaveChanges();
            CurrentSaCode = null;
            CurrentToLocation = (long)s.ToInventoryLocationId; 
            CurrentFromLocation = (long)s.FromInventoryLocationId;
            CurrentHeaderId = s.InventoryTxId;
            return Json(s.StockAdjCode);
        }

        public IActionResult CreateList()
        {

            var li = from s in _context.Suppliers.Where(a => a.ActiveFlag == "Y")
                     join b in _context.Products on s.SupplierId equals b.SupplierId
                     select new
                     {
                         text = s.Name,
                         value = b.Description

                     };
            return Json(li);
        }
        public IActionResult CreateProductSkuList()
        {
            var SaInternalTransferData = from a in _context.Products
                                select new
                                {
                                    text = a.Sku,
                                    value = a.Description

                                };
            return Json(SaInternalTransferData);
        }
        public IActionResult CreateProductName()
        {
            var SaInternalTransferData = from a in _context.Products
                                join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                                select new
                                {
                                    label = a.ProductId,
                                    value = a.Description


                                };
            return Json(SaInternalTransferData);
        }
        public IActionResult CreateLocationList()
        {
            var SaInternalTransferData = from a in _context.InventoryLocations
                                select new
                                {
                                    value = a.InventoryLocationId,
                                    text = a.Description
                                };
            return Json(SaInternalTransferData);
        }
        public IActionResult CreateReasonsList()
        {
            var SaInternalTransferData = from a in _context.InventoryTxReasons
                                select new
                                {
                                    value = a.InventoryTxReasonId,
                                    text = a.Description
                                };
            return Json(SaInternalTransferData);
        }
    }
}
