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

        public static Boolean showPage = false;
        public static string searchDate = DateTime.Today.ToShortDateString();
        public static string Notes;
        public static long CurrentHeaderId;
        public static string CurrentSaCode;
        public static long InventoryTxCurrentId;
        public static long CurrentFromLocation;
        private readonly WETT_DBContext _context;
        public SaOutboundController(WETT_DBContext context)
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
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         //  join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                         where c.InventoryTxTypeId == 3
                         select new SaOutboundViewModel
                         {
                             InventoryTxId = b.InventoryTxId,
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = e.Sku,
                             SupplierName = f.Name,
                             ProductId = e.ProductId,
                             ProductName = e.Description,
                             //InventoryLocationId = d.InventoryLocationId,
                             //     InventoryTxReasonsId = g.InventoryTxReasonId,
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

            var AllsaOutboundData = from b in _context.InventoryTxDetails
                                    join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                    join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                    //join d in _context.InventoryLocations on a.ToInventoryLocationId equals d.InventoryLocationId
                                    join e in _context.Products on b.ProductId equals e.ProductId
                                    join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                    //  join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                                    where c.InventoryTxTypeId == 3
                                    select new SaOutboundViewModel
                                    {
                                        InventoryTxId = b.InventoryTxId,
                                        InventoryTxDetailId = b.InventoryTxDetailId,
                                        ProductSku = e.Sku,
                                        SupplierName = f.Name,
                                        ProductId = e.ProductId,
                                        ProductName = e.Description,
                                        //InventoryLocationId = d.InventoryLocationId,
                                        //   InventoryTxReasonsId = g.InventoryTxReasonId,
                                        Amount = b.Amount,
                                        InventoryTxTypeId = c.InventoryTxTypeId,
                                        Comments = b.Comments,
                                        Date = a.Date, //.ToShortDateString(),
                                        SaCode = a.StockAdjCode
                                    };
            var SaOutboundData = AllsaOutboundData;



            //bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            //if (issearch)
            //    foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
            //    {
            //        switch (rule.field)
            //        {
            //            case "date":

            //                SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.Date.Equals(DateTime.Parse(rule.data)));
            //                searchDate = rule.data;
            //                break;
            //            case "comments":

            //                SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.Comments.Contains(rule.data));
            //                SaOutboundData = (IQueryable<SaOutboundViewModel>)SaOutboundData.Where(w => w.Date.Equals(DateTime.Parse(searchDate)));


            //                Notes = rule.data;
            //                break;
            //        }
            //    }
            if (CurrentSaCode != null)
            {
                SaOutboundData = SaOutboundData.Where(w => w.SaCode == CurrentSaCode);
            }
            else
            {
                if (showPage == true)
                {
                    //this is the type of transaction id
                   // SaOutboundData = SaOutboundData.Where(w => w.InventoryTxTypeId == 3);
                    SaOutboundData = SaOutboundData.Where(w => w.InventoryTxId == InventoryTxCurrentId);

                }
                else
                {
                    //this is to hide all transactons by type
                    SaOutboundData = SaOutboundData.Where(w => w.InventoryTxId == -1);
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
        public JsonResult Update(SaOutboundViewModel p)
        {
            Product s = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.FromInventoryLocationId = CurrentFromLocation;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            r.Comments = p.Comments;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(SaOutboundViewModel p)
        {
            showPage = true;
            Product s = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = new InventoryTxDetail
            {
                Comments = p.Comments,
                FromInventoryLocationId = CurrentFromLocation,
                ProductId = s.ProductId,
                Amount = p.Amount,
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
                InventoryTxTypeId = 3,          //hard coded transaction type id for now
                StockAdjCode = "OT",
                //add in extra cols here******************************************
                ShippingLocationId = (long)Convert.ToDouble(li[1]),
                TruckingCompanyId = (long)Convert.ToDouble(li[2]),
                Probill = li[3],
                FromInventoryLocationId = (long)Convert.ToDouble(li[4]),
                Comments = li[5]
                //*****************************************************************
            };
            //+date+'/'+destination+'/'+ truckingCompany+'/'+ probillNo+'/'+ fromLocationDropdown+'/'+ notes, function(data) {
            _context.InventoryTxes.Add(s);
            _context.SaveChanges();
            InventoryTxCurrentId = s.InventoryTxId;
            s.StockAdjCode = s.StockAdjCode + s.InventoryTxId;
            _context.SaveChanges();
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
            var saOutboundData = from a in _context.Products
                                 select new
                                 {
                                     text = a.Sku,
                                     value = a.Description

                                 };
            return Json(saOutboundData);
        }
        public IActionResult CreateProductName()
        {
            var saOutboundData = from a in _context.Products
                                 join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                                 select new
                                 {
                                     label = a.ProductId,
                                     value = a.Description


                                 };
            return Json(saOutboundData);
        }
        public IActionResult CreateLocationList()
        {
            var saOutboundData = from a in _context.InventoryLocations
                                 select new
                                 {
                                     value = a.InventoryLocationId,
                                     text = a.Description
                                 };
            return Json(saOutboundData);
        }
        public IActionResult CreateReasonsList()
        {
            var saOutboundData = from a in _context.InventoryTxReasons
                                 select new
                                 {
                                     value = a.InventoryTxReasonId,
                                     text = a.Description
                                 };
            return Json(saOutboundData);
        }
        public IActionResult CreateTruckingList()
        {
            var invAdjData = from a in _context.TruckingCompanies
                             select new
                             {
                                 value = a.TruckingCompanyId,
                                 text = a.Name
                             };
            return Json(invAdjData);
        }
        public IActionResult CreateDestinationList()
        {
            var invAdjData = from a in _context.ShippingLocations
                             select new
                             {
                                 value = a.ShippingLocationId,
                                 text = a.Name
                             };
            return Json(invAdjData);
        }

    }
}
