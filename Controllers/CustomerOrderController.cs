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
        public static long CurrentOrderId;
        public static string Notes;
        public static long CurrentCustomerOrderId;
        private readonly WETT_DBContext _context;
        public CustomerOrderController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string CustomerOrderID)
        {
            if (CustomerOrderID != null)
            {
                CurrentOrderId = (long)Convert.ToDouble(CustomerOrderID);
                CurrentCustomerOrderId = CurrentOrderId;
            }
            else
            {
                //CurrentOrderId = -1;
                CurrentCustomerOrderId = -1;
            }
        

            var result = from a in _context.CustomerOrders
                                          join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
                                          join c in _context.Products on b.ProductId equals c.ProductId
                                          where a.CustomerOrderId == CurrentCustomerOrderId
                         select new CustomerOrderViewModel
                                          {
                                              CustomerOrderDtlsID = b.CustomerOrderDetailId,
                                              CustomerOrderID = a.CustomerOrderId,
                                              ProductID = c.ProductId,
                                              ProductSku = c.Sku,
                                              ProductDesc = c.Description,
                                              StockQty = 0,
                                              QtyOrdered = b.QtyOrdered,
                                              QtyFulfilled = b.QtyFulfilled,
                                              Notes = b.Notes
                                          };
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {
            var AllCustomerOrderData = from a in _context.CustomerOrders
                                          join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
                                          join c in _context.Products on b.ProductId equals c.ProductId
                                          where a.CustomerOrderId == CurrentCustomerOrderId
                                       select new CustomerOrderViewModel
                                          {
                                              CustomerOrderDtlsID = b.CustomerOrderDetailId,
                                              CustomerOrderID = a.CustomerOrderId,
                                              ProductID = c.ProductId,
                                              ProductSku = c.Sku,
                                              ProductDesc = c.Description,
                                              StockQty = 0,
                                              QtyOrdered = b.QtyOrdered,
                                              QtyFulfilled = b.QtyFulfilled,
                                              Notes = b.Notes
                                          };
            var CustomerOrderData = AllCustomerOrderData;
            if (CurrentOrderId != -1)
            {
                CustomerOrderData = CustomerOrderData.Where(w => w.CustomerOrderID == CurrentOrderId);
            }
            


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
                CustomerOrderData = (IQueryable<CustomerOrderViewModel>)CustomerOrderData.OrderByDescending(t => t.ProductDesc);
                CustomerOrderData = CustomerOrderData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                CustomerOrderData = (IQueryable<CustomerOrderViewModel>)CustomerOrderData.OrderBy(t => t.ProductDesc);
                CustomerOrderData = (IQueryable<CustomerOrderViewModel>)CustomerOrderData.Skip(currentPageIndex * request.rows).Take(request.rows);
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
        public JsonResult Update(CustomerOrderViewModel p)
        {
            Product s = _context.Products.Single(a => a.Description == p.ProductDesc);
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(a => a.CustomerOrderDetailId == p.CustomerOrderDtlsID);
                r.ProductId = s.ProductId;
                r.QtyOrdered = p.QtyOrdered;
                r.QtyFulfilled = p.QtyFulfilled;
                r.Notes = p.Notes;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(CustomerOrderViewModel p)
        {
            Product s = _context.Products.Single(a => a.Description == p.ProductDesc);
            CustomerOrderDetail r = new CustomerOrderDetail
            {
                CustomerOrderId= CurrentCustomerOrderId,
                ProductId = s.ProductId,
                QtyOrdered = p.QtyOrdered,
                QtyFulfilled = p.QtyFulfilled,
                Notes = p.Notes

            };

            _context.CustomerOrderDetails.Add(r);
            _context.SaveChanges();


            return Json(true);
        }
        public JsonResult Delete(long id)
        {
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(e => e.CustomerOrderDetailId == id);
            _context.CustomerOrderDetails.Remove(r);
            _context.SaveChanges();


            return Json(true);
        }
        public IActionResult CreateHeader(string data)
        {
            var li = data.Split("/");
            CustomerOrder s = new CustomerOrder
            {
              CustomerId = (long)Convert.ToDouble(li[0]),
              OrderNumber = li[1],
              DateOrdered =  DateTime.Parse(li[2]),
              CustomerOrderStatusId = (long)Convert.ToDouble(li[3]),
              CarrierId = (long)Convert.ToDouble(li[4]),
              Driver = li[5],
              DsSlipNumber = li[6],
              DeliveryReqDate = DateTime.Parse(li[7]),
              SpecialInstructions = li[8],

            };

            _context.CustomerOrders.Add(s);
            _context.SaveChanges();
            CurrentCustomerOrderId = s.CustomerOrderId;
            return Json(true);
        }

        public IActionResult CreateCustomerList()
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
    }
}

