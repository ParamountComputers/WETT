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
    public class saDamageRecoupController : Controller
    {
        //classs variables
        //public static Boolean showPage = false;
        public static string CurrentSaCode;
        public static string CurrentNotes;
        public static DateTime CurrentDate;
        public static long InventoryTxCurrentId;
        private readonly WETT_DBContext _context;

        public saDamageRecoupController(WETT_DBContext context)
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
                         join e in _context.ProductMasters on b.ProductId equals e.ProductId
                         join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                         join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                         join h in _context.ProductRegulatorLiq on b.ProductId equals h.ProductId
                         where a.InventoryTxId == InventoryTxCurrentId
                         select new SaDamageRecoupViewModel
                         {
                             InventoryTxId = b.InventoryTxId,
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = h.Sku,
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
            var AllSaDamageRecoup = from b in _context.InventoryTxDetails
                                    join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                    join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                    join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                                    join e in _context.ProductMasters on b.ProductId equals e.ProductId
                                    join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                    join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                                    join h in _context.ProductRegulatorLiq on b.ProductId equals h.ProductId
                                    where a.InventoryTxId == InventoryTxCurrentId
                                    select new SaDamageRecoupViewModel
                                    {
                                        InventoryTxId = b.InventoryTxId,
                                        InventoryTxDetailId = b.InventoryTxDetailId,
                                        ProductSku = h.Sku,
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
            var saDamagerecoup = AllSaDamageRecoup;
            //checks if sent a specific code from invTxSummary if not show everything else
            if (CurrentSaCode != null)
            {
                saDamagerecoup = saDamagerecoup.Where(w => w.SaCode == CurrentSaCode);
                InventoryTx r = _context.InventoryTxes.Single(e => e.StockAdjCode == CurrentSaCode);
                InventoryTxCurrentId = r.InventoryTxId;
            }


            int totalRecords = saDamagerecoup.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                saDamagerecoup = (IQueryable<SaDamageRecoupViewModel>)saDamagerecoup.OrderByDescending(t => t.ProductName);
                saDamagerecoup = saDamagerecoup.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                saDamagerecoup = (IQueryable<SaDamageRecoupViewModel>)saDamagerecoup.OrderBy(t => t.ProductName);
                saDamagerecoup = (IQueryable<SaDamageRecoupViewModel>)saDamagerecoup.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = saDamagerecoup
            };

            return Json(jsonData);
        }
        public JsonResult Update(SaDamageRecoupViewModel p)
        {
            //updates the Inventory details to new entries 
            ProductMaster s = _context.ProductMasters.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.ToInventoryLocationId = p.InventoryLocationId;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            r.Comments = p.Comments;
            r.UpdateUserid = User.Identity.Name;
            r.UpdateTimestamp = DateTime.Now;
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
                    InventoryTxTypeId = 1,
                    InsertUserId = User.Identity.Name,
                    InsertTimestamp = DateTime.Now,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserId = User.Identity.Name,
                    StockAdjCode = "IA"
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
                s.Comments = CurrentNotes;
                s.Date = CurrentDate;
                s.InsertTimestamp = DateTime.Now;
                s.InsertUserId = User.Identity.Name;
                s.UpdateUserId = User.Identity.Name;
                s.UpdateTimestamp = DateTime.Now;
                _context.SaveChanges();
            }
            ProductMaster c = _context.ProductMasters.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = new InventoryTxDetail
            {
                Comments = p.Comments,
                ToInventoryLocationId = p.InventoryLocationId,
                ProductId = c.ProductId,
                Amount = p.Amount,
                InventoryTxId = InventoryTxCurrentId,
                InventoryTxReasonId = p.InventoryTxReasonsId,
                InsertTimestamp = DateTime.Now,
                InsertUserid = User.Identity.Name,
                UpdateTimestamp = DateTime.Now,
                UpdateUserid = User.Identity.Name,

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
            var saDamagerecoup = from a in _context.ProductRegulatorLiq
                             select new
                             {
                                 text = a.Sku,
                                 value = a.Description
                             };
            return Json(saDamagerecoup);
        }
        public IActionResult CreateProductName()
        {
            var saDamagerecoup = from a in _context.ProductMasters
                             select new
                             {
                                 value = a.SupplierId,
                                 text = a.Description
                             };
            return Json(saDamagerecoup);
        }
        public IActionResult CreateLocationList()
        {
            var saDamagerecoup = from a in _context.InventoryLocations
                             select new
                             {
                                 value = a.InventoryLocationId,
                                 text = a.Description
                             };
            return Json(saDamagerecoup);
        }
        public IActionResult CreateReasonsList()
        {
            var saDamagerecoup = from a in _context.InventoryTxReasons
                             select new
                             {
                                 value = a.InventoryTxReasonId,
                                 text = a.Description
                             };
            return Json(saDamagerecoup);
        }
    }
}