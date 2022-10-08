
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
        public class SaDamageRecoupController : Controller
        {
            //classs variables
            //public static Boolean showPage = false;
            public static string searchDate = DateTime.Today.ToShortDateString();
            public static string CurrentSaCode;
            public static string CurrentNotes;
            public static DateTime CurrentDate;
            public static string PrevNotes;
            public static DateTime PrevDate;
            public static long InventoryTxCurrentId;
            private readonly WETT_DBContext _context;

            public SaDamageRecoupController(WETT_DBContext context)
            {
                _context = context;
            }
            //references to database 
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
                             join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                             where a.InventoryTxId == InventoryTxCurrentId
                             select new SaDamageRecoupViewModel
                             {
                                 InventoryTxId = b.InventoryTxId,
                                 InventoryTxDetailId = b.InventoryTxDetailId,
                                 ProductSku = e.Sku,
                                 SupplierName = f.Name,
                                 SupplierId = f.SupplierId,
                                 ProductId = e.ProductId,
                                 ProductName = e.Description,
                                 InventoryLocationId = d.InventoryLocationId,
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
                //sets up grid with proper data
                var AllSaDamageRecoupData = from b in _context.InventoryTxDetails
                                    join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                    join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                    join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                                    join e in _context.Products on b.ProductId equals e.ProductId
                                    join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                    join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                                    where a.InventoryTxId == InventoryTxCurrentId
                                    select new SaDamageRecoupViewModel
                                    {
                                        InventoryTxId = b.InventoryTxId,
                                        InventoryTxDetailId = b.InventoryTxDetailId,
                                        ProductSku = e.Sku,
                                        SupplierName = f.Name,
                                        SupplierId = f.SupplierId,
                                        ProductId = e.ProductId,
                                        ProductName = e.Description,
                                        InventoryLocationId = d.InventoryLocationId,
                                        InventoryTxReasonsId = g.InventoryTxReasonId,
                                        Amount = b.Amount,
                                        InventoryTxTypeId = c.InventoryTxTypeId,
                                        Comments = b.Comments,
                                        Date = a.Date, //.ToShortDateString(),
                                        SaCode = a.StockAdjCode
                                    };
                var SaDamageRecoupData = AllSaDamageRecoupData;
                //checks if sent a specific code from invTxSummary if not show everything else
                if (CurrentSaCode != null)
                {
                    SaDamageRecoupData = SaDamageRecoupData.Where(w => w.SaCode == CurrentSaCode);
                    InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
                    InventoryTxCurrentId = r.InventoryTxId;
                }


                int totalRecords = SaDamageRecoupData.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
                int currentPageIndex = request.page - 1;

                //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
                if (request.sord.ToUpper() == "DESC")
                {
                    SaDamageRecoupData = (IQueryable<SaDamageRecoupViewModel>)SaDamageRecoupData.OrderByDescending(t => t.ProductName);
                    SaDamageRecoupData = SaDamageRecoupData.Skip(currentPageIndex * request.rows).Take(request.rows);
                }
                else
                {
                    SaDamageRecoupData = (IQueryable<SaDamageRecoupViewModel>)SaDamageRecoupData.OrderBy(t => t.ProductName);
                    SaDamageRecoupData = (IQueryable<SaDamageRecoupViewModel>)SaDamageRecoupData.Skip(currentPageIndex * request.rows).Take(request.rows);
                }
                var jsonData = new
                {
                    total = totalPages,
                    request.page,
                    records = totalRecords,
                    rows = SaDamageRecoupData
                };

                return Json(jsonData);
            }
            public JsonResult Update(SaDamageRecoupViewModel p)
            {
                //updates the Inventory details to new entries 
                Product s = _context.Products.Single(a => a.Description == p.ProductName);
                InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
                r.ToInventoryLocationId = p.InventoryLocationId;
                r.ProductId = s.ProductId;
                r.Amount = p.Amount;
                r.Comments = p.Comments;
                _context.SaveChanges();
                return Json(true);
            }
            public JsonResult Add(SaDamageRecoupViewModel p)
            {
                if (CurrentSaCode == null)
                {
                    InventoryTx s = new InventoryTx
                    {
                        Date = CurrentDate,
                        Comments = CurrentNotes,
                        InventoryTxTypeId = 2,
                        StockAdjCode = "DR"
                    };
                    _context.InventoryTxes.Add(s);
                    _context.SaveChanges();
                    PrevNotes = s.Comments;
                    PrevDate = s.Date;
                    InventoryTxCurrentId = s.InventoryTxId;
                    s.StockAdjCode = s.StockAdjCode + s.InventoryTxId;
                    _context.SaveChanges();
                    CurrentSaCode = s.StockAdjCode;
                }
                else
                {
                    InventoryTx s = _context.InventoryTxes.Single(a => a.InventoryTxId == InventoryTxCurrentId);
                    s.Comments = CurrentNotes;
                    s.Date = CurrentDate;
                    _context.SaveChanges();
                }
                Product c = _context.Products.Single(a => a.Description == p.ProductName);
                InventoryTxDetail r = new InventoryTxDetail
                {
                    Comments = p.Comments,
                    ToInventoryLocationId = p.InventoryLocationId,
                    ProductId = c.ProductId,
                    Amount = p.Amount,
                    InventoryTxId = InventoryTxCurrentId,
                    InventoryTxReasonId = p.InventoryTxReasonsId,

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
            public JsonResult SaCode()
            {
                InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
                var headerInfo = new
                {
                    comments = r.Comments,
                    sacode = CurrentSaCode,
                    date = r.Date.ToShortDateString()
                };
                if (CurrentSaCode != null)
                {
                    return Json(headerInfo);
                }
                return Json(null);
            }
            public IActionResult CreateHeader(string data)
            {
                var li = data.Split("/");
                CurrentDate = DateTime.Parse(li[0]);
                CurrentNotes = li[1];

                return Json(true);
            }
            public IActionResult DisplaySACode()
            {
                InventoryTx temp = _context.InventoryTxes.Single(a => a.InventoryTxId == InventoryTxCurrentId);
                return Json(temp.StockAdjCode);
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
                var SaDamageRecoupData = from a in _context.Products
                                 select new
                                 {
                                     text = a.Sku,
                                     value = a.Description
                                 };
                return Json(SaDamageRecoupData);
            }
            public IActionResult CreateProductName()
            {
                var SaDamageRecoupData = from a in _context.Products
                                 select new
                                 {
                                     value = a.SupplierId,
                                     text = a.Description
                                 };
                return Json(SaDamageRecoupData);
            }
            public IActionResult CreateLocationList()
            {
                var SaDamageRecoupData = from a in _context.InventoryLocations
                                 select new
                                 {
                                     value = a.InventoryLocationId,
                                     text = a.Description
                                 };
                return Json(SaDamageRecoupData);
            }
            public IActionResult CreateReasonsList()
            {
                var SaDamageRecoupData = from a in _context.InventoryTxReasons
                                 select new
                                 {
                                     value = a.InventoryTxReasonId,
                                     text = a.Description
                                 };
                return Json(SaDamageRecoupData);
            }
        }
    }