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
    public class CustomerOrderController : Controller
    {
        public static Boolean showPage = false;
        public static string searchDate = DateTime.Today.ToShortDateString();
        public static string CurrentSaCode;
        public static string Notes;
        public static long CurrentHeaderId;
        public static long InventoryTxCurrentId;
        private readonly WETT_DBContext _context;
        public CustomerOrderController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string SaCode)
        {
            CurrentSaCode = SaCode;
            InventoryTxCurrentId = -1;

        var result = from a in _context.CustomerOrders
                         join b in _context.Customers on a.CustomerId equals b.CustomerId
                         join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
                         join d in _context.Carriers on a.CarrierId equals d.CarrierId
                         select new CustomerOrder
                         {
                            CustomerOrderId = a.CustomerOrderId,
                            CustomerId = b.CustomerId,
                            CustomerOrderStatusId = c.CustomerOrderStatusId,
                            CarrierId = d.CarrierId,
                            OrderNumber = a.OrderNumber,
                            DateOrdered = a.DateOrdered,
                            Driver = a.Driver,
                            DsSlipNumber = a.DsSlipNumber,
                            DeliveryReqDate = (DateTime)a.DeliveryReqDate,
                            SpecialInstructions = a.SpecialInstructions,
                           

                         };
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {
            var AllCustomerOrderData = from a in _context.CustomerOrders
                         join b in _context.Customers on a.CustomerId equals b.CustomerId
                         join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
                         join d in _context.Carriers on a.CarrierId equals d.CarrierId
                         select new CustomerOrder
                         {
                             CustomerOrderId = a.CustomerOrderId,
                             CustomerId = b.CustomerId,
                             CustomerOrderStatusId = c.CustomerOrderStatusId,
                             CarrierId = d.CarrierId,
                             OrderNumber = a.OrderNumber,
                             DateOrdered = a.DateOrdered,
                             Driver = a.Driver,
                             DsSlipNumber = a.DsSlipNumber,
                             DeliveryReqDate = (DateTime)a.DeliveryReqDate,
                             SpecialInstructions = a.SpecialInstructions,


                         };
            var CustomerOrderData = AllCustomerOrderData;



            //bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            //if (issearch)
            //    foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
            //    {
            //        switch (rule.field)
            //        {
            //            case "date":

            //                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Date.Equals(DateTime.Parse(rule.data)));
            //                searchDate = rule.data;
            //                break;
            //            case "comments":

            //                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Comments.Contains(rule.data));
            //                invAdjData = (IQueryable<invAdjViewModel>)invAdjData.Where(w => w.Date.Equals(DateTime.Parse(searchDate)));


            //                Notes = rule.data;
            //                break;
            //        }
            //    }



            int totalRecords = CustomerOrderData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                CustomerOrderData = (IQueryable<CustomerOrder>)CustomerOrderData.OrderByDescending(t => t.OrderNumber);
                CustomerOrderData = CustomerOrderData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                CustomerOrderData = (IQueryable<CustomerOrder>)CustomerOrderData.OrderBy(t => t.OrderNumber);
                CustomerOrderData = (IQueryable<CustomerOrder>)CustomerOrderData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = CustomerOrderData
            };

            return Json(jsonData);
        }
        public JsonResult Update(CustomerOrder p)
        {
            CustomerOrder r = _context.CustomerOrders.Single(a => a.CustomerOrderId == p.CustomerOrderId);
            r.CustomerId = p.CustomerId;
            r.CustomerOrderStatusId = p.CustomerOrderStatusId;
            r.CarrierId = p.CarrierId;
            r.OrderNumber = p.OrderNumber;
            r.DateOrdered = p.DateOrdered;
            r.Driver = p.Driver;
            r.DsSlipNumber = p.DsSlipNumber;
            r.DeliveryReqDate = p.DeliveryReqDate;
            r.SpecialInstructions = p.SpecialInstructions;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(CustomerOrder p)
        {
            showPage = true;
            _context.CustomerOrders.Add(p);
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
        //public IActionResult CreateHeader(string data)
        //{
        //    var li = data.Split("/");
        //    InventoryTx s = new InventoryTx
        //    {
        //        Date = DateTime.Parse(li[0]),
        //        Comments = li[1],
        //        InventoryTxTypeId = 1,
        //        StockAdjCode = "IA"
        //    };

        //    _context.InventoryTxes.Add(s);
        //    _context.SaveChanges();
        //    InventoryTxCurrentId = s.InventoryTxId;
        //    s.StockAdjCode = s.StockAdjCode + s.InventoryTxId;
        //    _context.SaveChanges();
        //    CurrentHeaderId = s.InventoryTxId;
        //    return Json(s.StockAdjCode);
        //}

        public IActionResult CreatetCustomerList()
        {
            var invAdjData = from a in _context.Customers
                             select new
                             {
                                 value = a.CustomerId,
                                 text = a.Name
                             };
            return Json(invAdjData);
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
        public IActionResult CreatetCustomerOrderStatusList()
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

