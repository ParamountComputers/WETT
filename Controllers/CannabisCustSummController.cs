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
    public class CannabisCustSummController : Controller
    {
        public static Boolean showPage;
        //public static string searchDate = DateTime.Today.ToShortDateString();
        public static string startSearchDate = "";
        public static string endSearchDate = "";
        public static long StatusOrderId;
        public static long carrierId;
        //public static string Notes;
        //public static long CurrentHeaderId;
        private readonly WETT_DBContext _context;
        public CannabisCustSummController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            showPage = false;
            StatusOrderId = -1;
            var result = from a in _context.CustomerOrders
                         join b in _context.Customers on a.CustomerId equals b.CustomerId
                        join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
                         where a.CustomerOrderStatusId == StatusOrderId && a.LobCode.Trim() == "CAN"
                         select new CanCustSummViewModel
                         {
                             CustomerOrderID = a.CustomerOrderId,
                             OrderDate = a.DateOrdered,
                           //  DelveryDate = a.DeliveryReqDate,
                             OrderNumber = a.OrderNumber,
                             LOBCode = a.LobCode,
                             Customer = b.Name,
                             City = b.City,
                             Instructions = a.SpecialInstructions,
                             Status = c.Description


                         };
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {


            var AllCanCustomerSummaryData = from a in _context.CustomerOrders
                         join b in _context.Customers on a.CustomerId equals b.CustomerId
                         join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
                         where a.CustomerOrderStatusId == StatusOrderId && a.LobCode.Trim() == "CAN"
                         select new CanCustSummViewModel
                         {
                             CustomerOrderID = a.CustomerOrderId,
                             OrderDate = a.DateOrdered,
                          //   DelveryDate = a.DeliveryReqDate,
                             OrderNumber = a.OrderNumber,
                             LOBCode = a.LobCode,
                             Customer = b.Name,
                             City = b.City,
                             Instructions = a.SpecialInstructions,
                             Status = c.Description


                         };
            var CanCustomerSummaryData = AllCanCustomerSummaryData;
            if (showPage != false)
            {
               CanCustomerSummaryData = CanCustomerSummaryData.Where(x => x.OrderDate >= DateTime.Parse(startSearchDate) && x.OrderDate <= DateTime.Parse(endSearchDate));
            }


            int totalRecords = CanCustomerSummaryData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                CanCustomerSummaryData = (IQueryable<CanCustSummViewModel>)CanCustomerSummaryData.OrderByDescending(t => t.OrderDate);
                CanCustomerSummaryData = CanCustomerSummaryData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                CanCustomerSummaryData = (IQueryable<CanCustSummViewModel>)CanCustomerSummaryData.OrderBy(t => t.OrderDate);
                CanCustomerSummaryData = (IQueryable<CanCustSummViewModel>)CanCustomerSummaryData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = CanCustomerSummaryData
            };

            return Json(jsonData);
        }
        public JsonResult Update(CanCustSummViewModel p)
        {
            //Customer s = _context.Customers.Single(a => a.Name == p.Customer);
            CustomerOrder r = _context.CustomerOrders.Single(a => a.CustomerId == p.CustomerOrderID);
            //r.DateOrdered = p.OrderDate;
            //r.DeliveryReqDate = p.DelveryDate;
            //r.OrderNumber = p.OrderNumber;
            //r.CustomerId = s.CustomerId;
            //r.CarrierId = p.CarrierID;
            r.UpdateUserId = User.Identity.Name;
            r.UpdateTimestamp = DateTime.Now;
            r.SpecialInstructions = p.Instructions;
            //r.CustomerOrderStatusId = p.Status;

            _context.SaveChanges();
            return Json(true);
        }
        
        public IActionResult CreateSearch(string data)
        {
            showPage = true;
            var li = data.Split("/");
            startSearchDate = li[0];
            endSearchDate = li[1];
            StatusOrderId = (long)Convert.ToDouble(li[2]);
           // carrierId = (long)Convert.ToDouble(li[3]);

            return Json(true);
        }
        //public IActionResult CreateCarrierList()
        //{
        //    var invAdjData = from a in _context.Carriers
        //                     select new
        //                     {
        //                         value = a.CarrierId,
        //                         text = a.Name

        //                     };
        //    return Json(invAdjData);
        //}


        public IActionResult CreateStatusList()
        {
            var invAdjData = from a in _context.CustomerOrderStatuses
                             select new
                             {
                                 value = a.CustomerOrderStatusId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
    }
}

