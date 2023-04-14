
using DocumentFormat.OpenXml.InkML;
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
        public class ReceivedCannabisOrderController : Controller
        {
            public static Boolean showPage;
            public static string orderNumber;
            public static long CurrentCustomerOrderId;
            private readonly WETT_DBContext _context;

            public ReceivedCannabisOrderController(WETT_DBContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Index(string CustomerOrderID)
            {
                if (CustomerOrderID != null)
                {
                    CurrentCustomerOrderId = (long)Convert.ToDouble(CustomerOrderID);
                }
                else
                {
                    CurrentCustomerOrderId = -1;
                }


                var result = from a in _context.CustomerOrders
                             join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
                             join c in _context.ProductMasters on b.ProductId equals c.ProductId
                             join d in _context.ProductRegulatorCan on b.ProductId equals d.ProductId
                             where a.CustomerOrderId == CurrentCustomerOrderId && a.LobCode.Trim() == "CAN"
                             select new CustomerOrderViewModel
                             {
                                 CustomerOrderDtlsID = b.CustomerOrderDetailId,
                                 CustomerOrderID = a.CustomerOrderId,
                                 ProductID = c.ProductId,
                                 ProductSku = d.Sku,
                                 ProductDesc = c.Description,
                                 StockQty = 0,
                                 QtyOrdered = b.QtyOrdered,
                                 QtyFulfilled= b.QtyFulfilled,
                                 Notes = b.Notes
                             };
                return View(result);
            }


            public JsonResult GetAll(JqGridViewModel request)
            {
                var AllCustomerOrderData = from a in _context.CustomerOrders
                                           join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
                                           join c in _context.ProductMasters on b.ProductId equals c.ProductId
                                           join d in _context.ProductRegulatorCan on b.ProductId equals d.ProductId
                                           where a.LobCode.Trim() == "CAN" && a.OrderNumber == orderNumber //CurrentCustomerOrderId.ToString()
                                           select new CustomerOrderViewModel
                                           {
                                               CustomerOrderDtlsID = b.CustomerOrderDetailId,
                                               CustomerOrderID = a.CustomerOrderId,
                                               OrderNumber = a.OrderNumber,
                                               ProductID = c.ProductId,
                                               ProductSku = d.Sku,
                                               ProductDesc = c.Description,
                                               StockQty = 0,
                                               QtyOrdered = b.QtyOrdered,
                                               QtyFulfilled = b.QtyFulfilled,
                                               Notes = b.Notes
                                           };
                var CustomerOrderData = AllCustomerOrderData;
                if (CurrentCustomerOrderId != -1)
                {
                    CustomerOrderData = CustomerOrderData.Where(w => w.OrderNumber == CurrentCustomerOrderId.ToString());

                }

            if (showPage != false)
            {
                CustomerOrderData = CustomerOrderData.Where(w => w.OrderNumber == orderNumber);
            }

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
                ProductMaster s = _context.ProductMasters.Single(a => a.Description == p.ProductDesc);
                CustomerOrderDetail r = _context.CustomerOrderDetails.Single(a => a.CustomerOrderDetailId == p.CustomerOrderDtlsID);
                r.ProductId = p.ProductID;
                r.QtyOrdered = p.QtyOrdered;
                r.QtyFulfilled = p.QtyFulfilled;
                r.Notes = p.Notes;
                r.UpdateTimestamp = DateTime.Now;
                r.UpdateUserid = User.Identity.Name;
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

            public IActionResult CreateSearch(string data)
            {
                showPage = true;
                var li = data.Split("/");
                orderNumber = li[0];
               

                return Json(true);
            }
        public IActionResult CreateHeader(string data)
            {
                //var li = data.Split("/");

                orderNumber = data;
             var newOrder = _context.CustomerOrders.Single(e => e.OrderNumber == orderNumber);
            CurrentCustomerOrderId = newOrder.CustomerOrderId;
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
                var invAdjData = from a in _context.ProductMasters
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
                var invAdjData = from a in _context.ProductRegulatorCan
                                 select new
                                 {
                                     text = a.Sku,
                                     value = a.Description

                                 };
                return Json(invAdjData);
            }
            public IActionResult CreateStockQtyList()
            {
                var invAdjData = from a in _context.ProductMasters
                                 join d in _context.Inventories on a.ProductId equals d.ProductId
                                 where d.InventoryLocationId == 1
                                 select new
                                 {
                                     text = d.Count,
                                     value = a.Description

                                 };
                return Json(invAdjData);
            }
        }
    }