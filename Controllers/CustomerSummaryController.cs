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
    public class CustomerSummaryController : Controller
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
        public CustomerSummaryController(WETT_DBContext context)
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
                         join d in _context.Carriers on a.CarrierId equals d.CarrierId
                         where a.CustomerOrderStatusId == StatusOrderId && a.LobCode.Trim() == "LIQ"
                         select new CustomerSummaryViewModel
                         {
                             CustomerOrderID = a.CustomerOrderId,
                             OrderDate = a.DateOrdered,
                             DelveryDate = a.DeliveryReqDate,
                             OrderNumber = a.OrderNumber,
                             LOBCode = a.LobCode,
                             Customer = b.Name,
                             City = b.City,
                             Carrier = d.Name,
                             CarrierId = d.CarrierId,
                             CarrierDesc = d.Name,
                             Instructions = a.SpecialInstructions,
                             Status = c.Description


                         };
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {


            var AllCustomerSummaryData = from a in _context.CustomerOrders
                         join b in _context.Customers on a.CustomerId equals b.CustomerId
                         join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
                         join d in _context.Carriers on a.CarrierId equals d.CarrierId
                         where a.CustomerOrderStatusId == StatusOrderId && a.LobCode.Trim() == "LIQ"
                         select new CustomerSummaryViewModel
                         {
                             CustomerOrderID = a.CustomerOrderId,
                             OrderDate = a.DateOrdered,
                             DelveryDate = a.DeliveryReqDate,
                             OrderNumber = a.OrderNumber,
                             LOBCode = a.LobCode,
                             Customer = b.Name,
                             City = b.City,
                             Carrier = d.Name,
                             CarrierId = d.CarrierId,
                             CarrierDesc = d.Name,
                             Instructions = a.SpecialInstructions,
                             Status = c.Description


                         };
            var CustomerSummaryData = AllCustomerSummaryData;
            if (showPage != false)
            {
                if (carrierId == 0) { 
                CustomerSummaryData = CustomerSummaryData.Where(x => x.OrderDate >= DateTime.Parse(startSearchDate) && x.OrderDate <= DateTime.Parse(endSearchDate));
                }else {
                    CustomerSummaryData = CustomerSummaryData.Where(x => x.OrderDate >= DateTime.Parse(startSearchDate) && x.OrderDate <= DateTime.Parse(endSearchDate) &&  x.CarrierId == carrierId);
                }
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



            int totalRecords = CustomerSummaryData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                CustomerSummaryData = (IQueryable<CustomerSummaryViewModel>)CustomerSummaryData.OrderByDescending(t => t.OrderDate);
                CustomerSummaryData = CustomerSummaryData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                CustomerSummaryData = (IQueryable<CustomerSummaryViewModel>)CustomerSummaryData.OrderBy(t => t.OrderDate);
                CustomerSummaryData = (IQueryable<CustomerSummaryViewModel>)CustomerSummaryData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = CustomerSummaryData
            };

            return Json(jsonData);
        }
        public JsonResult Update(CustomerSummaryViewModel p)
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
            carrierId = (long)Convert.ToDouble(li[3]);

            return Json(true);
        }
        public IActionResult CreateCarrierList()
        {
            var invAdjData = from a in _context.Carriers
                             select new
                             {
                                 value = a.CarrierId,
                                 text = a.Name

                             };
            return Json(invAdjData);
        }


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

