using DocumentFormat.OpenXml.ExtendedProperties;
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
    public class SaStockReceivedController : Controller
    {
        public static DateTime CurrentDate;
        public static string CurrentNotes;
        public static string CurrentSealNumber;
        public static long CurrentLTolocationsId;
        public static long CurrentLSupplierId;
        public static string CurrentProbill;
        public static long CurrentTruckingCompany;
        public static string CurrentSaCode;
        public static string CurrentPurchaseOrder;
        public static long InventoryTxCurrentId;
        public static long CurrentToLocation;
        private readonly WETT_DBContext _context;
        public SaStockReceivedController(WETT_DBContext context)
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
                         join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                         join e in _context.ProductMasters on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         join g in _context.TruckingCompanies on a.TruckingCompanyId equals g.TruckingCompanyId
                         join h in _context.ProductRegulatorLiqs on b.ProductId equals h.ProductId
                         where a.InventoryTxId == InventoryTxCurrentId && b.Deleted == false
                         select new SaStockReceivedViewModel
                         {
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = h.Sku,
                             SupplierName = f.Name,
                             InventoryTxId = b.InventoryTxId,
                             ProductId = e.ProductId,
                             ProductName = h.Description,
                             InventoryLocationId = d.InventoryLocationId,
                             Amount = b.Amount,
                             Comments = b.Comments,
                             Date = a.Date, //.ToShortDateString(),
                             SaCode = a.StockAdjCode

                         };
            return View(result);
        }

        public JsonResult GetAll(JqGridViewModel request)
        {
            
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var AllSaStockReceivedData = from b in _context.InventoryTxDetails
                                         join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                         join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                         join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                                         join e in _context.ProductMasters on b.ProductId equals e.ProductId
                                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                         join g in _context.TruckingCompanies on a.TruckingCompanyId equals g.TruckingCompanyId
                                         join h in _context.ProductRegulatorLiqs on b.ProductId equals h.ProductId
                                         where a.InventoryTxId == InventoryTxCurrentId && b.Deleted == false
                                         select new SaStockReceivedViewModel
                                         {
                                             InventoryTxDetailId = b.InventoryTxDetailId,
                                             ProductSku = h.Sku,
                                             SupplierName = f.Name,
                                             InventoryTxId = b.InventoryTxId,
                                             ProductId = e.ProductId,
                                             ProductName = h.Description,
                                             InventoryLocationId = d.InventoryLocationId,
                                             Amount = b.Amount,
                                             Comments = b.Comments,
                                             Date = a.Date, //.ToShortDateString(),
                                             SaCode = a.StockAdjCode

                                         };
            var SaStockReceivedData =AllSaStockReceivedData;


            int totalRecords = SaStockReceivedData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                SaStockReceivedData = (IQueryable<SaStockReceivedViewModel>)SaStockReceivedData.OrderByDescending(t => t.ProductName);
                SaStockReceivedData = SaStockReceivedData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                SaStockReceivedData = (IQueryable<SaStockReceivedViewModel>)SaStockReceivedData.OrderBy(t => t.ProductName);
                SaStockReceivedData = (IQueryable<SaStockReceivedViewModel>)SaStockReceivedData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = SaStockReceivedData
            };

            return Json(jsonData);
        }
        public JsonResult Add(SaExciseDutyViewModel p)
        {
            if (InventoryTxCurrentId == -1)
            {
                InventoryTx s = new InventoryTx
                {
                    Date = CurrentDate,
                    PurchaseOrder = CurrentPurchaseOrder,
                    ToInventoryLocationId = CurrentLTolocationsId,
                    Comments = CurrentNotes,
                    SupplierId = CurrentLSupplierId,
                    TruckingCompanyId = CurrentTruckingCompany,
                    Seal = CurrentSealNumber,
                    Probill= CurrentProbill,
                    InventoryTxTypeId = 7,
                    InsertTimestamp = DateTime.Now,
                    InsertUserId = User.Identity.Name,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserId = User.Identity.Name,
                    StockAdjCode = "SR"
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
                //s.ToInventoryLocationId = CurrentToLocation;
                s.PurchaseOrder = CurrentPurchaseOrder;
                s.ToInventoryLocationId = CurrentLTolocationsId;
                s.InsertTimestamp = DateTime.Now;
                s.InsertUserId = User.Identity.Name;
                s.UpdateTimestamp = DateTime.Now;
                s.UpdateUserId = User.Identity.Name;
                _context.SaveChanges();
            }
            ProductRegulatorLiq c = _context.ProductRegulatorLiqs.Single(a => a.Description == p.ProductName);
            //ProductMaster c = _context.ProductMasters.Single(a => a.ProductId == p.ProductId);
            InventoryTxDetail r = new InventoryTxDetail
                {
                    Comments = p.Comments,
                    ToInventoryLocationId = CurrentLTolocationsId,
                    ProductId = c.ProductId,
                    Amount = p.Amount,
                    InventoryTxId = InventoryTxCurrentId,
                    InsertTimestamp = DateTime.Now,
                    InsertUserid = User.Identity.Name,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserid = User.Identity.Name,
                    Deleted = false
                };
                _context.InventoryTxDetails.Add(r);
                _context.SaveChanges();

                return Json(true);

         }
        public IActionResult CreateHeader(string data)
        {
            var li = data.Split("/");
            CurrentDate = DateTime.Parse(li[0]);
            CurrentNotes = li[1];
            CurrentTruckingCompany = (long)Convert.ToDouble(li[2]);
            CurrentPurchaseOrder = li[3];
            CurrentSealNumber = li[4];
            CurrentProbill = li[5];
            CurrentLTolocationsId = (long)Convert.ToDouble(li[6]);
            CurrentLSupplierId = (long)Convert.ToDouble(li[7]);
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
                    truckingCompanyDropdown = r.TruckingCompanyId,
                    sealNo = r.Seal,
                    locations = r.ToInventoryLocationId,
                    truckerProbillNumber = r.Probill,
                    purchaseOrder = r.PurchaseOrder,
                    supplier = r.SupplierId
                };
                return Json(headerInfo);
            }
            return Json(null);
        }
        public JsonResult Update(SaStockReceivedViewModel p)
        {

            //ProductMaster s = _context.ProductMasters.Single(a => a.Description == p.ProductName);
           // ProductMaster s = _context.ProductMasters.Single(a => a.ProductId == p.ProductId);
            ProductRegulatorLiq s = _context.ProductRegulatorLiqs.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.ToInventoryLocationId = CurrentToLocation;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            r.Comments = p.Comments;
            r.UpdateTimestamp = DateTime.Now;
            r.UpdateUserid = User.Identity.Name;
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

        public IActionResult CreateList()
        {

            var li = from s in _context.Suppliers.Where(a => a.ActiveFlag == "Y")
                     where s.LobCode == "LIQ"
                     select new
                     {
                         text = s.Name,
                         value = s.SupplierId

                     };
            return Json(li.OrderByDescending(t => t.value));
        }
        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from a in _context.ProductRegulatorLiqs
                             where a.ActiveFlag == true
                             select new
                             {
                                 text = a.Sku,
                                 value = a.Description

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductName()
        {
            var li = from a in _context.ProductMasters
                             join b in _context.ProductRegulatorLiqs on a.ProductId equals b.ProductId
                             where a.ActiveFlag == true
                             select new
                             {
                                 value =a.SupplierId,
                                 text = b.Description,

                             };
            return Json(li.OrderByDescending(t => t.value));
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
    }
}
