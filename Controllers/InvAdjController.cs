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
    public class invAdjController : Controller
    {
        public static Boolean showPage = false;
        public static string searchDate = DateTime.Today.ToShortDateString();
        public static string CurrentSaCode;
        public static string Notes;
        public static long CurrentHeaderId;
        public static long InventoryTxCurrentId;
        private readonly WETT_DBContext _context;
        public invAdjController(WETT_DBContext context)
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
                         join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId
                         select new invAdjViewModel
                         {
                             InventoryTxId = b.InventoryTxId,
                             InventoryTxDetailId = b.InventoryTxDetailId,
                             ProductSku = e.Sku,
                             SupplierName = f.Name,
                             ProductId = e.ProductId,
                             ProductName = e.Description,
                             InventoryLocationId = d.InventoryLocationId,
                             InventoryTxReasonsId = g.InventoryTxReasonId,
                             Amount = b.Amount,
                             InventoryTxTypeId = c.InventoryTxTypeId,
                             Comments = a.Comments,
                             Date = a.Date, //.ToShortDateString(),
                             SaCode = a.StockAdjCode

                         };
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {

            var AllInvAdjData = from b in _context.InventoryTxDetails
                                        join a in _context.InventoryTxes on b.InventoryTxId equals a.InventoryTxId
                                        join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                        join d in _context.InventoryLocations on b.ToInventoryLocationId equals d.InventoryLocationId
                                        join e in _context.Products on b.ProductId equals e.ProductId
                                        join f in _context.Suppliers on e.SupplierId equals f.SupplierId
                                        join g in _context.InventoryTxReasons on b.InventoryTxReasonId equals g.InventoryTxReasonId

                                        select new invAdjViewModel
                                        {
                                            InventoryTxId = b.InventoryTxId,
                                            InventoryTxDetailId = b.InventoryTxDetailId,
                                            ProductSku = e.Sku,
                                            SupplierName = f.Name,
                                            ProductId = e.ProductId,
                                            ProductName = e.Description,
                                            InventoryLocationId = d.InventoryLocationId,
                                            InventoryTxReasonsId = g.InventoryTxReasonId,
                                            Amount = b.Amount,
                                            InventoryTxTypeId = c.InventoryTxTypeId,
                                            Comments = a.Comments,
                                            Date = a.Date, //.ToShortDateString(),
                                            SaCode = a.StockAdjCode
                                        };
            var invAdjData = AllInvAdjData;

            if (CurrentSaCode != null)
            {
                invAdjData = invAdjData.Where(w => w.SaCode == CurrentSaCode);
            }
            else
            {
                if (showPage == true)
                {
                    //this is the type of transaction id
                    invAdjData = invAdjData.Where(w => w.InventoryTxTypeId == 1);
                    invAdjData = invAdjData.Where(w => w.InventoryTxId == InventoryTxCurrentId);

                }
                else
                {
                    //this is to hide all transactons by type
                    invAdjData = invAdjData.Where(w => w.InventoryTxId == -1);
                }
            }



            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "date":

                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Date.Equals(DateTime.Parse(rule.data)));
                            searchDate = rule.data;
                            break;
                        case "comments":

                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Comments.Contains(rule.data));
                            invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Date.Equals(DateTime.Parse(searchDate)));


                            Notes = rule.data;
                            break;
                    }
                }



            int totalRecords = invAdjData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.OrderByDescending(t => t.ProductName);
                invAdjData = invAdjData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.OrderBy(t => t.ProductName);
                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = invAdjData
            };

            return Json(jsonData);
        }
        public JsonResult Update(invAdjViewModel p)
        {
            Product s = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            r.ToInventoryLocationId = p.InventoryLocationId;
            r.ProductId = s.ProductId;
            r.Amount = p.Amount;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(invAdjViewModel p)
        {
            showPage = true;
            Product s = _context.Products.Single(a => a.Description == p.ProductName);
            InventoryTxDetail r = new InventoryTxDetail
            {
                //comments = p.Comments,
                ToInventoryLocationId = p.InventoryLocationId,
                ProductId = s.ProductId,
                Amount = p.Amount,
                InventoryTxId = CurrentHeaderId,
                InventoryTxReasonId = p.InventoryTxReasonsId
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
                Comments = li[1],
                InventoryTxTypeId = 1,
                StockAdjCode = "IA"
            };

            _context.InventoryTxes.Add(s);
            _context.SaveChanges();
            InventoryTxCurrentId = s.InventoryTxId;
            s.StockAdjCode = s.StockAdjCode + s.InventoryTxId;
            _context.SaveChanges();
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
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             select new
                             {
                                 label = a.ProductId,
                                 value = a.Description


                             };
            return Json(invAdjData);
        }
        public IActionResult CreateLocationList()
        {
            var invAdjData = from a in _context.InventoryLocations
                             select new
                             {
                                 value = a.InventoryLocationId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
        public IActionResult CreateReasonsList()
        {
            var invAdjData = from a in _context.InventoryTxReasons
                             select new
                             {
                                 value = a.InventoryTxReasonId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
    }
    }
