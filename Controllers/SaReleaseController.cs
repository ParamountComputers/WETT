﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WETT.Data;
using WETT.Models;

namespace WETT.Controllers
{
    public class SaReleaseController : Controller
    {

        public static DateTime CurrentDate;
        public static string Notes;
        public static string CurrentProbill;
        public static string CurrentPurchaseOrder;
        public static string CurrentPortEntry;
        public static string CurrentPrevTransNo;
        public static string CurrentTransNumber;
        public static string CurrentSaCode;
        public static long CurrentToLocation;
        public static long InventoryTxCurrentId;
        private readonly WETT_DBContext _context;
        public SaReleaseController(WETT_DBContext context)
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
                         join e in _context.Products on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         where a.InventoryTxId == InventoryTxCurrentId
                         select new SaReleaseViewModel
                         {
                             InventoryTxId = b.InventoryTxId,
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = e.Sku,
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

            var AllSaReleaseData = from b in _context.InventoryTxDetails
                                   join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                   join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                   join e in _context.Products on b.ProductId equals e.ProductId
                                   join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                   where a.InventoryTxId == InventoryTxCurrentId
                                   select new SaReleaseViewModel
                                   {
                                       InventoryTxId = b.InventoryTxId,
                                       InventoryTxDetailId = b.InventoryTxDetailId,
                                       ProductSku = e.Sku,
                                       SupplierName = f.Name,
                                       ProductId = e.ProductId,
                                       ProductName = e.Description,
                                       Amount = b.Amount,
                                       InventoryTxTypeId = c.InventoryTxTypeId,
                                       Comments = b.Comments,
                                       Date = a.Date, //.ToShortDateString(),
                                       SaCode = a.StockAdjCode
                                   };
            var SaReleaseData = AllSaReleaseData;
            if (CurrentSaCode != null)
            {
                SaReleaseData = SaReleaseData.Where(w => w.SaCode == CurrentSaCode);
                InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
                InventoryTxCurrentId = r.InventoryTxId;
            }


            int totalRecords = SaReleaseData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                SaReleaseData = (IQueryable<SaReleaseViewModel>)SaReleaseData.OrderByDescending(t => t.ProductName);
                SaReleaseData = SaReleaseData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                SaReleaseData = (IQueryable<SaReleaseViewModel>)SaReleaseData.OrderBy(t => t.ProductName);
                SaReleaseData = (IQueryable<SaReleaseViewModel>)SaReleaseData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = SaReleaseData
            };

            return Json(jsonData);
        }
        public JsonResult Update(SaReleaseViewModel p)
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
        public JsonResult Add(SaExciseDutyViewModel p)
        {
            if (CurrentSaCode == null)
            {
                InventoryTx s = new InventoryTx
                {
                    Date = CurrentDate,
                    InventoryTxTypeId = 4,          //hard coded transaction type id for now
                    StockAdjCode = "RB",
                    //add in extra cols here******************************************
                    PurchaseOrder = CurrentPurchaseOrder,
                    ToInventoryLocationId = CurrentToLocation,
                    PortOfEntry = CurrentPortEntry,
                    PreviousTransactionNo = CurrentPrevTransNo,
                    TransactionNo = CurrentPrevTransNo,
                    Comments = Notes,
                    Probill = CurrentProbill,
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
                s.PurchaseOrder = CurrentPurchaseOrder;
                s.PortOfEntry = CurrentPortEntry;
                s.PreviousTransactionNo = CurrentPrevTransNo;
                s.TransactionNo = CurrentTransNumber;
                s.ToInventoryLocationId = CurrentToLocation;
                s.Probill = CurrentProbill;
                _context.SaveChanges();
            }
            Product c = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = new InventoryTxDetail
            {
                Comments = p.Comments,
                ToInventoryLocationId = CurrentToLocation,
                FromInventoryLocationId = 2,
                ProductId = c.ProductId,
                Amount = p.Amount,
                InventoryTxId = InventoryTxCurrentId
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
            CurrentTransNumber =li[2];
            CurrentPrevTransNo = li[3];
            CurrentPurchaseOrder = li[4];
            CurrentPortEntry = li[5];
            CurrentProbill = li[6];
            CurrentToLocation = (long)Convert.ToDouble(li[7]);
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
                transNumber = r.TransactionNo,
                prevTransNo = r.PreviousTransactionNo,
                locations = r.ToInventoryLocationId,
                probill = r.Probill,
                portEntry = r.PortOfEntry,
                purchaseOrder = r.PurchaseOrder
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
            var SaReleaseData = from a in _context.Products
                                select new
                                {
                                    text = a.Sku,
                                    value = a.Description

                                };
            return Json(SaReleaseData);
        }
        public IActionResult CreateProductName()
        {
            var SaReleaseData = from a in _context.Products
                                 select new
                                 {
                                     value = a.SupplierId,
                                     text = a.Description
                                 };
            return Json(SaReleaseData);
        }

        public IActionResult CreateLocationList()
        {
            var SaReleaseData = from a in _context.InventoryLocations.Where(a => a.InventoryLocationId == 1 || a.InventoryLocationId == 4)
                                select new
                                {
                                    value = a.InventoryLocationId,
                                    text = a.Description
                                };
            return Json(SaReleaseData);
        }
        public IActionResult CreateReasonsList()
        {
            var SaReleaseData = from a in _context.InventoryTxReasons
                                select new
                                {
                                    value = a.InventoryTxReasonId,
                                    text = a.Description
                                };
            return Json(SaReleaseData);
        }
    }
}
