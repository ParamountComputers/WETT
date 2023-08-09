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
        public static string Notes;
        public static string CurrentSaCode;
        public static long InventoryTxCurrentId;
        public static long CurrentToLocation;
        public static long CurrentFromLocation;
        public static DateTime CurrentDate;
        private readonly WETT_DBContext _context;
        public SaInternalTransferController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string InventoryTxId)
        {
            if (InventoryTxId != null)
            {
                InventoryTxCurrentId = (long)Convert.ToDouble(InventoryTxId);
            }
            else
            {
                InventoryTxCurrentId = -1;
            }
            var result = from b in _context.InventoryTxDetails
                         join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                         join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                         join d in _context.InventoryLocations on a.ToInventoryLocationId equals d.InventoryLocationId
                         join d2 in _context.InventoryLocations on a.FromInventoryLocationId equals d2.InventoryLocationId
                         join e in _context.ProductMasters on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                         join h in _context.ProductRegulatorLiqs on b.ProductId equals h.ProductId
                         where a.InventoryTxId == InventoryTxCurrentId && b.Deleted == false
                         select new SaInternalTransferViewModel
                         {
                             InventoryTxId = b.InventoryTxId,
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = h.Sku,
                             SupplierName = f.Name,
                             ProductId = e.ProductId,
                             ProductName = h.Description,
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
                                            join e in _context.ProductMasters on b.ProductId equals e.ProductId
                                            join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                            join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                                            join h in _context.ProductRegulatorLiqs on b.ProductId equals h.ProductId
                                            where a.InventoryTxId == InventoryTxCurrentId && b.Deleted == false
                                            select new SaInternalTransferViewModel
                                            {
                                                InventoryTxId = b.InventoryTxId,
                                                InventoryTxDetailId = b.InventoryTxDetailId,
                                                ProductSku = h.Sku,
                                                SupplierName = f.Name,
                                                ProductId = e.ProductId,
                                                ProductName = h.Description,
                                                toInventoryLocationId = (long)a.ToInventoryLocationId,
                                                fromInventoryLocationId = (long)a.FromInventoryLocationId,
                                                InventoryTxReasonsId = g.InventoryTxReasonId,
                                                Amount = b.Amount,
                                                InventoryTxTypeId = c.InventoryTxTypeId,
                                                Comments = b.Comments,
                                                Date = a.Date, //.ToShortDateString(),
                                                SaCode = a.StockAdjCode

                                            };
            var SaInternalTransferData = AllSaInternalTransferData;


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
            //ProductMaster s = _context.ProductMasters.Single(a => a.Description == p.ProductName);
            ProductRegulatorLiq s = _context.ProductRegulatorLiqs.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.ToInventoryLocationId = CurrentToLocation;
            r.FromInventoryLocationId = CurrentFromLocation;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            r.Comments = p.Comments;
            r.InventoryTxReasonId = p.InventoryTxReasonsId;
            r.UpdateTimestamp = DateTime.Now;
            r.UpdateUserid = User.Identity.Name;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(SaExciseDutyViewModel p)
        {
            if (InventoryTxCurrentId == -1)
            {
                InventoryTx s = new InventoryTx
                {
                    Date = CurrentDate,
                    InventoryTxTypeId = 6,          //hard coded transaction type id for now
                    StockAdjCode = "IT",
                    //add in extra cols here******************************************            
                    ToInventoryLocationId = CurrentToLocation,
                    FromInventoryLocationId = CurrentFromLocation,
                    InsertTimestamp = DateTime.Now,
                    InsertUserId = User.Identity.Name,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserId = User.Identity.Name,
                    Comments = Notes
                    //*****************************************************************
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
                s.ToInventoryLocationId = CurrentToLocation;
                s.FromInventoryLocationId = CurrentFromLocation;
                s.InsertTimestamp = DateTime.Now;
                s.InsertUserId = User.Identity.Name;
                s.UpdateTimestamp = DateTime.Now;
                s.UpdateUserId = User.Identity.Name;
                _context.SaveChanges();
            }
            ProductRegulatorLiq c = _context.ProductRegulatorLiqs.Single(a => a.Description == p.ProductName);
           // ProductMaster c = _context.ProductMasters.Single(a => a.ProductId == p.ProductId);
            InventoryTxDetail r = new InventoryTxDetail
            {
                Comments = p.Comments,
                ToInventoryLocationId = CurrentToLocation,
                FromInventoryLocationId = CurrentFromLocation,
                ProductId = c.ProductId,
                Amount = p.Amount,
                InventoryTxReasonId = p.InventoryTxReasonsId,
                InsertTimestamp = DateTime.Now,
                InsertUserid = User.Identity.Name,
                UpdateTimestamp = DateTime.Now,
                UpdateUserid = User.Identity.Name,
                InventoryTxId = InventoryTxCurrentId,
                Deleted= false,
            };

            _context.InventoryTxDetails.Add(r);
            _context.SaveChanges();

            return Json(true);

        }
        public JsonResult Delete(long id)
        {
            InventoryTxDetail r = _context.InventoryTxDetails.Single(e => e.InventoryTxDetailId == id);
            r.Deleted = true;
            _context.SaveChanges();


            return Json(true);
        }
        public IActionResult CreateHeader(string data)
        {
            var li = data.Split("/");
            CurrentDate = DateTime.Parse(li[0]);
            Notes = li[1];
            CurrentToLocation = (long)Convert.ToDouble(li[2]);
            CurrentFromLocation = (long)Convert.ToDouble(li[3]);
            return Json(true);
        }
        public JsonResult SaCode()
        {
            if (InventoryTxCurrentId != -1)
            {
                InventoryTx r = _context.InventoryTxes.Single(e => e.InventoryTxId == InventoryTxCurrentId);
                var headerInfo = new
                {
                    comments = r.Comments,
                    sacode = CurrentSaCode,
                    date = r.Date,
                    toLocation = r.ToInventoryLocationId,
                    fromLocation = r.FromInventoryLocationId
                };
                return Json(headerInfo);
            }
            return Json(null);
        }
        public IActionResult DisplaySACode()
        {
            InventoryTx temp = _context.InventoryTxes.Single(a => a.InventoryTxId == InventoryTxCurrentId);
            return Json(temp.StockAdjCode);
        }
        public IActionResult CreateList()
        {

            var li = from a in _context.ProductMasters
                     join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                     join c in _context.ProductRegulatorLiqs on a.ProductId equals c.ProductId
                     where b.ActiveFlag == "Y" && b.LobCode == "LIQ"
                     select new
                     {
                         text = b.Name,
                         value = c.Description

                     };
            return Json(li);
        }
        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from a in _context.ProductRegulatorLiqs
                             select new
                             {
                                 text = a.Sku,
                                 value = a.Description
                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductName()
        {
            var invAdjData = from a in _context.ProductMasters
                             join b in _context.ProductRegulatorLiqs on a.ProductId equals b.ProductId
                             where a.LobCode == "LIQ"
                             select new
                             {
                                 value = a.SupplierId,
                                 text = b.Description
                             };
            return Json(invAdjData);
        }
        public IActionResult CreateToLocationList()
        {
            var SaInternalTransferData = from a in _context.InventoryLocations
                                         select new
                                         {
                                             value = a.InventoryLocationId,
                                             text = a.Description
                                         };
            return Json(SaInternalTransferData);
        }
        public IActionResult CreateFromLocationList()
        {
            var SaInternalTransferData = from a in _context.InventoryLocations.Where(a => a.InventoryLocationId != 1 && a.InventoryLocationId != 2)
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