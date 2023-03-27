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

        public static DateTime CurrentDate;
        public static string Notes;
        public static string CurrentSaCode;
        public static long CurrentTruckingCompany;
        public static long CurrentLocation;
        public static string CurrentProbill;
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
                         join e in _context.ProductMasters on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         join h in _context.ProductRegulatorLiq on b.ProductId equals h.ProductId
                         where a.InventoryTxId == InventoryTxCurrentId
                         select new SaOutboundViewModel
                         {
                             InventoryTxId = b.InventoryTxId,
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = h.Sku,
                             SupplierName = f.Name,
                             ProductId = e.ProductId,
                             ProductName = e.Description,
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
                                    join e in _context.ProductMasters on b.ProductId equals e.ProductId
                                    join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                    join h in _context.ProductRegulatorLiq on b.ProductId equals h.ProductId
                                    where a.InventoryTxId == InventoryTxCurrentId
                                    select new SaOutboundViewModel
                                    {
                                        InventoryTxId = b.InventoryTxId,
                                        InventoryTxDetailId = b.InventoryTxDetailId,
                                        ProductSku = h.Sku,
                                        SupplierName = f.Name,
                                        ProductId = e.ProductId,
                                        ProductName = e.Description,
                                        Amount = b.Amount,
                                        InventoryTxTypeId = c.InventoryTxTypeId,
                                        Comments = b.Comments,
                                        Date = a.Date, //.ToShortDateString(),
                                        SaCode = a.StockAdjCode

                                    };
            var SaOutboundData = AllsaOutboundData;

            if (CurrentSaCode != null)
            {
                SaOutboundData = SaOutboundData.Where(w => w.SaCode == CurrentSaCode);
                InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
                InventoryTxCurrentId = r.InventoryTxId;
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
            ProductMaster s = _context.ProductMasters.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.FromInventoryLocationId = CurrentFromLocation;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            r.Comments = p.Comments;
            r.UpdateTimestamp = DateTime.Now;
            r.UpdateUserid = User.Identity.Name;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(SaExciseDutyViewModel p)
        {
            if (CurrentSaCode == null)
            {
                InventoryTx s = new InventoryTx
                {
                    Date = CurrentDate,
                    InventoryTxTypeId = 3,          //hard coded transaction type id for now
                    StockAdjCode = "OT",
                    //add in extra cols here******************************************
                    ShippingLocationId = CurrentLocation,
                    TruckingCompanyId = CurrentTruckingCompany,
                    Probill = CurrentProbill,
                    FromInventoryLocationId = CurrentFromLocation,
                    InsertTimestamp = DateTime.Now,
                    InsertUserId = User.Identity.Name,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserId = User.Identity.Name,
                    Comments = Notes
                };
                _context.InventoryTxes.Add(s);
                _context.SaveChanges();
                InventoryTxCurrentId = s.InventoryTxId;
                s.StockAdjCode = s.StockAdjCode + s.InventoryTxId;
                _context.SaveChanges();
                CurrentSaCode = s.StockAdjCode;
            }
            else
            {
                InventoryTx s = _context.InventoryTxes.Single(a => a.InventoryTxId == InventoryTxCurrentId);
                s.Comments = Notes;
                s.Date = CurrentDate;
                s.ShippingLocationId = CurrentLocation;
                s.Probill = CurrentProbill;
                s.TruckingCompanyId = CurrentTruckingCompany;
                s.FromInventoryLocationId = CurrentFromLocation;
                s.InsertTimestamp = DateTime.Now;
                s.InsertUserId = User.Identity.Name;
                s.UpdateTimestamp = DateTime.Now;
                s.UpdateUserId = User.Identity.Name;
                _context.SaveChanges();
            }
            ProductMaster c = _context.ProductMasters.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = new InventoryTxDetail
            {
                Comments = p.Comments,
                FromInventoryLocationId = CurrentFromLocation,
                ProductId = c.ProductId,
                Amount = p.Amount,
                InsertTimestamp = DateTime.Now,
                InsertUserid = User.Identity.Name,
                UpdateTimestamp = DateTime.Now,
                UpdateUserid = User.Identity.Name,
                InventoryTxId = InventoryTxCurrentId
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
            CurrentDate = DateTime.Parse(li[0]);
            Notes = li[1];
            CurrentFromLocation = (long)Convert.ToDouble(li[2]);
            CurrentLocation = (long)Convert.ToDouble(li[3]);
            CurrentTruckingCompany = (long)Convert.ToDouble(li[4]);
            CurrentProbill = li[5];
            return Json(true);
        }
        public JsonResult SaCode()
        {
            if (CurrentSaCode == null)
            {
                return Json(null);
            }
            InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
            var headerInfo = new
            {
                comments = r.Comments,
                sacode = CurrentSaCode,
                date = r.Date.ToShortDateString(),
                fromLocation = r.FromInventoryLocationId,
                truckingComp = r.TruckingCompanyId,
                destDropdown = r.ShippingLocationId,
                probill = r.Probill
            };
            return Json(headerInfo);
        }
        public IActionResult DisplaySACode()
        {
            InventoryTx temp = _context.InventoryTxes.Single(a => a.InventoryTxId == InventoryTxCurrentId);
            return Json(temp.StockAdjCode);
        }


        public IActionResult CreateList()
        {

            var li = from s in _context.Suppliers.Where(a => a.ActiveFlag == "Y")
                     join b in _context.ProductMasters on s.SupplierId equals b.SupplierId
                     select new
                     {
                         text = s.Name,
                         value = b.Description

                     };
            return Json(li);
        }
        public IActionResult CreateProductSkuList()
        {
            var saOutboundData = from a in _context.ProductRegulatorLiq
                                 select new
                                 {
                                     text = a.Sku,
                                     value = a.Description

                                 };
            return Json(saOutboundData);
        }
        public IActionResult CreateProductName()
        {
            var saOutboundData = from a in _context.ProductMasters
                             select new
                             {
                                 value = a.SupplierId,
                                 text = a.Description
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
