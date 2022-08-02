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
    public class InvTxSummaryController : Controller
    {
        public static Boolean showPage;
        //public static string searchDate = DateTime.Today.ToShortDateString();
        public static string startSearchDate="";
        public static string endSearchDate="";
        public static long inventoryTxType;
        //public static string Notes;
        //public static long CurrentHeaderId;
        private readonly WETT_DBContext _context;
        public InvTxSummaryController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            showPage = false;
            inventoryTxType = -1;
            var result = from a in _context.InventoryTxes 
                         join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                         where c.InventoryTxTypeId == inventoryTxType
                         select new InvTxSummaryViewModel
                         {
                             InventoryTxId = a.InventoryTxId,
                             InventoryTxTypeId = c.InventoryTxTypeId,
                             Comments = a.Comments,
                             Date = a.Date, //.ToShortDateString(),
                             SaCode = a.StockAdjCode

                         };
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {


            var AllInvTxSummaryData = from a in _context.InventoryTxes
                                join c in _context.InventoryTxTypes on a.InventoryTxTypeId equals c.InventoryTxTypeId
                                where c.InventoryTxTypeId== inventoryTxType
                                      select new InvTxSummaryViewModel
                                {
                                    InventoryTxId = a.InventoryTxId,
                                    InventoryTxTypeId = c.InventoryTxTypeId,
                                    Comments = a.Comments,
                                    Date = a.Date, //.ToShortDateString(),
                                    SaCode = a.StockAdjCode

                                };
            var InvTxSummaryData = AllInvTxSummaryData;
            if (showPage != false)
            {
                InvTxSummaryData = InvTxSummaryData.Where(x => x.Date >= DateTime.Parse(startSearchDate) && x.Date <= DateTime.Parse(endSearchDate));
            }



            //bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            //if (issearch)
            //    foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
            //    {
            //        switch (rule.field)
            //        {
            //            case "date":

            //                InvTxSummaryData = (IQueryable<InvTxSummaryViewModel>)InvTxSummaryData.Where(w => w.Date.Equals(DateTime.Parse(rule.data)));
            //                searchDate = rule.data;
            //                break;
            //            case "comments":

            //                InvTxSummaryData = (IQueryable<InvTxSummaryViewModel>)InvTxSummaryData.Where(w => w.Comments.Contains(rule.data));
            //                InvTxSummaryData = (IQueryable<InvTxSummaryViewModel>)InvTxSummaryData.Where(w => w.Date.Equals(DateTime.Parse(searchDate)));


            //                Notes = rule.data;
            //                break;
            //        }
            //    }



            int totalRecords = InvTxSummaryData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                InvTxSummaryData = (IQueryable<InvTxSummaryViewModel>)InvTxSummaryData.OrderByDescending(t => t.Date);
                InvTxSummaryData = InvTxSummaryData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                InvTxSummaryData = (IQueryable<InvTxSummaryViewModel>)InvTxSummaryData.OrderBy(t => t.Date);
                InvTxSummaryData = (IQueryable<InvTxSummaryViewModel>)InvTxSummaryData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = InvTxSummaryData
            };

            return Json(jsonData);
        }
        public JsonResult Update(InvTxSummaryViewModel p)
        {
            InventoryTx r = _context.InventoryTxes.Single(a => a.StockAdjCode == p.SaCode);
            r.Comments = p.Comments;
            _context.SaveChanges();
            return Json(true);
        }
        //public JsonResult Add(InvTxSummaryViewModel p)
        //{
        //    showPage = true;
        //    Product s = _context.Products.Single(a => a.Description == p.ProductName);
        //    InventoryTxDetail r = new InventoryTxDetail
        //    {
        //        //comments = p.Comments,
        //        ToInventoryLocationId = p.InventoryLocationId,
        //        ProductId = s.ProductId,
        //        Amount = p.Amount,
        //        InventoryTxId = CurrentHeaderId,
        //        InventoryTxReasonId = p.InventoryTxReasonsId
        //    };

        //    _context.InventoryTxDetails.Add(r);
        //    _context.SaveChanges();


        //    return Json(true);
        //}
        //public JsonResult Delete(long id)
        //{
        //    InventoryTxDetail r = _context.InventoryTxDetails.Single(e => e.InventoryTxDetailId == id);
        //    _context.InventoryTxDetails.Remove(r);
        //    _context.SaveChanges();


        //    return Json(true);
        //}
        public IActionResult CreateSearch(string data)
        {
            showPage = true;
            var li = data.Split("/");
            startSearchDate = li[0];
            endSearchDate= li[1];
            inventoryTxType= (long)Convert.ToDouble(li[2]);

            return Json(true);
        }

        public IActionResult CreateInvTxTypeList()
        {
            var invAdjData = from a in _context.InventoryTxTypes
                             select new
                             {
                                 value = a.InventoryTxTypeId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
    }
}

