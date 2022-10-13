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
        public static string CurrentSaCode;
        public static string Notes;
        public static long CurrentTruckingCompany;
        public static string CurrentPurchaseOrder;
        public static string CurrentSealNumber;
        public static string CurrentProbill;
        public static string CurrentLocations;
        public static long CurrentSupplier;
        public static long InventoryTxCurrentId;
        public static long CurrentToLocation;
        private readonly WETT_DBContext _context;
        public SaStockReceivedController(WETT_DBContext context)
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
                         join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         join g in _context.TruckingCompanies on a.TruckingCompanyId equals g.TruckingCompanyId
                         where a.InventoryTxId == InventoryTxCurrentId
                         select new SaStockReceivedViewModel
                         {
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = e.Sku,
                             SupplierName = f.Name,
                             InventoryTxId = b.InventoryTxId,
                             ProductId = e.ProductId,
                             ProductName = e.Description,
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
                                      join e in _context.Products on b.ProductId equals e.ProductId
                                      join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                       where a.InventoryTxId == InventoryTxCurrentId
                                         select new SaStockReceivedViewModel
                                      {
                                          InventoryTxId = b.InventoryTxId,
                                          InventoryTxDetailId = b.InventoryTxDetailId,
                                         ProductSku = e.Sku,
                                         SupplierName = f.Name,
                                         ProductId = e.ProductId,
                                         ProductName = e.Description,
                                         InventoryTxTypeId = c.InventoryTxTypeId,
                                          Amount = b.Amount,
                                         Comments = b.Comments,
                                         Date = a.Date, //.ToShortDateString(),
                                         SaCode = a.StockAdjCode

                                     };
            var SaStockReceivedData =AllSaStockReceivedData;
            if (CurrentSaCode != null)
            {
                SaStockReceivedData = SaStockReceivedData.Where(w => w.SaCode == CurrentSaCode);
                InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
                InventoryTxCurrentId = r.InventoryTxId;
            }



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
            if (CurrentSaCode == null)
            {
                InventoryTx s = new InventoryTx
                {
                    Date = CurrentDate,
                    Comments = Notes,
                    TruckingCompanyId = CurrentTruckingCompany,
                    PurchaseOrder = CurrentPurchaseOrder,
                    Seal = CurrentSealNumber,
                    Probill = CurrentProbill,
                    ToInventoryLocationId = CurrentToLocation,
                    SupplierId = CurrentSupplier,
                    InventoryTxTypeId = 7,
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
                s.Comments = Notes;
                s.Date = CurrentDate;
                s.ToInventoryLocationId = CurrentToLocation;
                s.Probill = CurrentProbill;
                s.PurchaseOrder = CurrentPurchaseOrder;
                s.TruckingCompanyId = CurrentTruckingCompany;
                s.Seal = CurrentSealNumber;
                s.ToInventoryLocationId = CurrentToLocation;
                s.SupplierId = CurrentSupplier;
                _context.SaveChanges();
            }
                Product c = _context.Products.Single(a => a.Description == p.ProductName);
                InventoryTxDetail r = new InventoryTxDetail
                {
                    Comments = p.Comments,
                    ToInventoryLocationId = CurrentToLocation,
                    ProductId = c.ProductId,
                    Amount = p.Amount,
                    InventoryTxId = InventoryTxCurrentId,
                };
                _context.InventoryTxDetails.Add(r);
                _context.SaveChanges();

                return Json(true);

         }
        public IActionResult CreateHeader(string data)
        {
            var li = data.Split("/");
            CurrentDate = DateTime.Parse(li[0]);
            Notes = li[1];
            CurrentTruckingCompany = (long)Convert.ToDouble(li[2]);
            CurrentPurchaseOrder = li[3];
            CurrentSealNumber = li[4];
            CurrentProbill = li[5];
            CurrentToLocation = (long)Convert.ToDouble(li[6]);
            CurrentSupplier = (long)Convert.ToDouble(li[7]);
            CurrentToLocation = (long)Convert.ToDouble(li[7]);
            return Json(true);
        }

        public JsonResult SaCode()
        {
            InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
            var headerInfo = new
            {
                comments = r.Comments,
                sacode = CurrentSaCode,
                date = r.Date.ToShortDateString(),
                truckingCompanyDropdown = r.TruckingCompanyId,
                sealNo = r.Seal,
                locations = r.ToInventoryLocationId,
                truckerProbillNumber = r.Probill,
                purchaseOrder = r.PurchaseOrder,
                supplier = r.SupplierId
                
            };
            if (CurrentSaCode != null)
            {
                return Json(headerInfo);
            }
            return Json(null);
        }
        public JsonResult Update(SaStockReceivedViewModel p)
        {

            Product s = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.ToInventoryLocationId = CurrentToLocation;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            r.Comments = p.Comments;
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
                         value = s.SupplierId

                     };
            return Json(li);
        }
        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from a in _context.Products
                             select new
                             {
                                 text = a.Sku,
                                 value = a.Description

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductName()
        {
            var invAdjData = from a in _context.Products
                             select new
                             {
                                 value =a.SupplierId,
                                 text = a.Description,

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
