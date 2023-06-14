using Azure.Core;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WETT.Data;
using WETT.Models;

namespace WETT.Controllers
{
    public class CannabisOrderEntryController : Controller
    {
        //public static long currentCustomer;
        //public static string currentOrderNumber;
        //public static DateTime currentDateOrdered;
        //public static DateTime currentDateShipped;
        //public static DateTime currentReceivedDate;
        //public static long currentCustomerOrderStatus;
        //public static string currentSpecialInstructions;
        //public static long currentCarrier;
        private static long CurrentCustomerOrderId;
        //public static long currentSupplierId;
        //public static IQueryable<CustomerList> customerList;
        private readonly WETT_DBContext _context;

        public CannabisOrderEntryController( WETT_DBContext context)
        {
           
            _context = context;
            //CurrentCustomerOrderId = 1;
        }

        public async Task<IActionResult> Index(string CustomerOrderID)
        {
            if (CustomerOrderID != null)
            {
                CurrentCustomerOrderId = (long)Convert.ToDouble(CustomerOrderID);
               // ViewData["CurrentFilter"] = CurrentCustomerOrderId;
            }
            else
            {
               // ViewData["CurrentFilter"] = CurrentCustomerOrderId;
                CurrentCustomerOrderId = -1;
            }

            var result = from a in _context.CustomerOrders
                         join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
                         join c in _context.ProductMasters on b.ProductId equals c.ProductId
                         join d in _context.ProductRegulatorCans on b.ProductId equals d.ProductId
                         where a.CustomerOrderId == CurrentCustomerOrderId && a.LobCode.Trim() == "CAN" //&& c.SupplierId == currentSupplierId
                         select new CustomerOrderViewModel
                         {
                             CustomerOrderDtlsID = b.CustomerOrderDetailId,
                             CustomerOrderID = a.CustomerOrderId,
                             ProductID = c.ProductId,
                             //ProductSku = d.Sku,
                             ProductDesc = d.Sku +" - "+ d.Description,
                             QtyOrdered = b.QtyOrdered,
                             //QtyFulfilled = b.QtyFulfilled,
                             Notes = b.Notes
                         };
            //    bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            //    if (issearch)
            //        foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
            //        {
            //            switch (rule.field)
            //            {
            //                case "CustomerOrderID":
            //                    result = (IQueryable<CustomerOrderViewModel>)result.Where(w => w.CustomerOrderID == (long)Convert.ToDouble(rule.data)).ToList();
            //                    break;
            //            }
            //        }
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {

            var AllCustomerOrderData = from a in _context.CustomerOrders
                                       join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
                                       join c in _context.ProductMasters on b.ProductId equals c.ProductId
                                       join d in _context.ProductRegulatorCans on b.ProductId equals d.ProductId
                                       where a.CustomerOrderId == CurrentCustomerOrderId && a.LobCode.Trim() == "CAN"  //&& c.SupplierId == currentSupplierId
                                       select new CustomerOrderViewModel
                                       {
                                           CustomerOrderDtlsID = b.CustomerOrderDetailId,
                                           CustomerOrderID = a.CustomerOrderId,
                                           ProductID = c.ProductId,
                                          // ProductSku = d.Sku,
                                           ProductDesc = d.Sku + " - " + d.Description,
                                           QtyOrdered = b.QtyOrdered,
                                           //QtyFulfilled= b.QtyFulfilled,
                                           Notes = b.Notes
                                       };
            var CustomerOrderData = AllCustomerOrderData;
            //if (CurrentCustomerOrderId != -1)
            //{
            //    CustomerOrderData = CustomerOrderData.Where(w => w.CustomerOrderID == CurrentCustomerOrderId);

            //}

            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "CustomerOrderID":
                            CustomerOrderData = (IQueryable<CustomerOrderViewModel>)CustomerOrderData.Where(w => w.CustomerOrderID == (long)Convert.ToDouble(rule.data)).ToList();
                            break;
                    }
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
            var a = p.ProductDesc.Split(" ");
            ProductRegulatorCan c = _context.ProductRegulatorCans.Single(b => b.Sku == a[0]);
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(a => a.CustomerOrderDetailId == p.CustomerOrderDtlsID);
            r.ProductId = c.ProductId;
            r.QtyOrdered = p.QtyOrdered;
            r.QtyFulfilled = p.QtyFulfilled;
            r.Notes = p.Notes;
            r.UpdateTimestamp = DateTime.Now;
            r.UpdateUserid = User.Identity.Name;
            _context.SaveChanges();
            return Json(true);
        }

        public JsonResult Add(CustomerOrderViewModel p)
        {
            //if (CurrentCustomerOrderId == -1)
            //{

            //    CustomerOrder s = new CustomerOrder
            //    {
            //        CustomerId = currentCustomer,
            //        OrderNumber = currentOrderNumber,
            //        DateOrdered = currentDateOrdered,
            //        DateReceived = currentReceivedDate,
            //        DateShipped = currentDateShipped,
            //        CustomerOrderStatusId = currentCustomerOrderStatus,
            //        SupplierId = currentSupplierId,
            //        //CarrierId = currentCarrier,
            //        SpecialInstructions = currentSpecialInstructions,
            //        //hard coded for now
            //        LobCode = "CAN",
            //        RegulatorCode = "MBLL",
            //        OrderSourceId = 1,
            //        CarrierId = 1,
            //        InsertTimestamp = DateTime.Now,
            //        InsertUserId = User.Identity.Name,
            //        UpdateTimestamp = DateTime.Now,
            //        UpdateUserId = User.Identity.Name,

            //    };

            //    _context.CustomerOrders.Add(s);
            //    _context.SaveChanges();
            //    CurrentCustomerOrderId = s.CustomerOrderId;
            //}
            //else
            //{
            //    CustomerOrder s = _context.CustomerOrders.Single(a => a.CustomerOrderId == CurrentCustomerOrderId);
            //    s.CustomerId = currentCustomer;
            //    s.DateShipped = currentDateShipped;
            //    s.OrderNumber = currentOrderNumber;
            //    s.DateOrdered = currentDateOrdered;
            //    s.DateReceived = currentReceivedDate;
            //    //s.CarrierId = currentCarrier;
            //    s.CustomerOrderStatusId = currentCustomerOrderStatus;
            //    s.SpecialInstructions = currentSpecialInstructions;
            //    s.UpdateTimestamp = DateTime.Now;
            //    s.UpdateUserId = User.Identity.Name;
            //    _context.SaveChanges();
            //}
            CustomerOrderDetail r = new CustomerOrderDetail
            {
                CustomerOrderId = CurrentCustomerOrderId,
                ProductId = p.ProductID,
                QtyOrdered = p.QtyOrdered,
                QtyFulfilled= p.QtyFulfilled,
                Notes = p.Notes,
                InsertTimestamp = DateTime.Now,
                InsertUserId = User.Identity.Name,
                UpdateTimestamp = DateTime.Now,
                UpdateUserid = User.Identity.Name,


            };

            _context.CustomerOrderDetails.Add(r);
            _context.SaveChanges();

            return Json(true);

        }
        public JsonResult Delete(int id)
        {
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(e => e.CustomerOrderDetailId == id);
            _context.CustomerOrderDetails.Remove(r);
            _context.SaveChanges();
            //////// if currentCustomerOderId is static this works to remove the header on deletion on last details//////////////////////////////////
            if (_context.CustomerOrderDetails.Where(w => w.CustomerOrderId.Equals(r.CustomerOrderId)).Any() == false)
            {
                CustomerOrder s = _context.CustomerOrders.Single(a => a.CustomerOrderId == r.CustomerOrderId);
                CurrentCustomerOrderId = -1;
                _context.CustomerOrders.Remove(s);
            }
            _context.SaveChanges();


            return Json(true);
        }
        public JsonResult SaCode()
        {

            if (CurrentCustomerOrderId != -1)
            {
                CustomerOrder r = _context.CustomerOrders.Single(e => e.CustomerOrderId == CurrentCustomerOrderId);
                var headerInfo = new
                {
                    customer = r.CustomerId,
                    orderNumber = r.OrderNumber,
                    dateOrdered = r.DateOrdered.ToString("yyyy-MM-dd"),
                    dateReceived = r.DateReceived.ToString("yyyy-MM-dd"),
                    supplier = r.SupplierId,
                    shippedDate = r.DateShipped.ToString("yyyy-MM-dd"),    
                    customerOrderStatus = r.CustomerOrderStatusId,
                    specialInstructions = r.SpecialInstructions,
                };
                return Json(headerInfo);
            }
            return Json(null);
        }
        public IActionResult CreateHeader(string data)
        {
            //customerList = from a in _context.Customers
            //                   // orderby a.Name
            //               select new CustomerList
            //               {
            //                   value = a.CustomerId,
            //                   text = a.MbllCustomerNo + " - " + a.Name
            //               };
            //CustomerList n = customerList.Single(x => x.text == li[0]);
            //currentCustomer = n.value;

            //var li = data.Split("/");

            //currentCustomerOrderStatus = (long)Convert.ToDouble(li[3]);
            //currentOrderNumber = li[1];
            //if (li[2] != "")
            //{
            //    currentDateOrdered = DateTime.Parse(li[2]);
            //}
            //if (li[6] != "")
            //{
            //    currentReceivedDate = DateTime.Parse(li[6]);
            //}
            //if (li[7] != "")
            //{
            //    currentDateShipped = DateTime.Parse(li[7]);
            //}
            //currentSpecialInstructions = li[4];
            //currentSupplierId = (long)Convert.ToDouble(li[5]);
            //currentCustomer = (long)Convert.ToDouble(li[0]);



            var li = data.Split("/");
            if (CurrentCustomerOrderId == -1)
            {

                CustomerOrder s = new CustomerOrder
                {
                    CustomerId = (long)Convert.ToDouble(li[0]),
                    OrderNumber = li[1],
                    CustomerOrderStatusId = (long)Convert.ToDouble(li[3]),
                    SupplierId = (long)Convert.ToDouble(li[5]),
                    //CarrierId = currentCarrier,
                    SpecialInstructions = li[4],
                    //hard coded for now
                    LobCode = "CAN",
                    RegulatorCode = "MBLL",
                    OrderSourceId = 1,
                    CarrierId = 1,
                    InsertTimestamp = DateTime.Now,
                    InsertUserId = User.Identity.Name,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserId = User.Identity.Name,

                };
                if (li[2] != "")
                {
                    s.DateOrdered = DateTime.Parse(li[2]);
                }
                if (li[6] != "")
                {
                    s.DateReceived = DateTime.Parse(li[6]);
                }
                if (li[7] != "")
                {
                    s.DateShipped = DateTime.Parse(li[7]);
                }

                _context.CustomerOrders.Add(s);
                _context.SaveChanges();
                CurrentCustomerOrderId = s.CustomerOrderId;
            }
            else
            {
                CustomerOrder s = _context.CustomerOrders.Single(a => a.CustomerOrderId == CurrentCustomerOrderId);
                s.CustomerId = (long)Convert.ToDouble(li[0]);
                s.OrderNumber = li[1];
                if (li[2] != "")
                {
                    s.DateOrdered = DateTime.Parse(li[2]);
                }
                if (li[6] != "")
                {
                    s.DateReceived = DateTime.Parse(li[6]);
                }
                if (li[7] != "")
                {
                    s.DateShipped = DateTime.Parse(li[7]);
                }
                //s.CarrierId = currentCarrier;
                s.CustomerOrderStatusId = (long)Convert.ToDouble(li[3]);
                s.SpecialInstructions = li[4];
                s.UpdateTimestamp = DateTime.Now;
                s.UpdateUserId = User.Identity.Name;
                _context.SaveChanges();
            }



            return Json(true);
            
        }
        public IActionResult CreateSupplierList()
        {
            var supplier = from a in _context.Suppliers
                             where a.ActiveFlag == "Y"
                             orderby a.Name
                             select new
                             {
                                 value = a.SupplierId,
                                 text = a.Name
                                
                             };
            return Json(supplier);
        }


        public IActionResult CreateCustomerList()
        {
            var customerList = from a in _context.Customers
                           orderby a.Name
                           select new CustomerList
                           {
                               value = a.CustomerId,
                               text = a.MbllCustomerNo + " - " + a.Name
                           };
            return Json(customerList);
        }
        public IActionResult CreateCarrierList()
        {
            var carrier = from a in _context.Carriers
                             orderby a.Name
                             select new
                             {
                                 value = a.CarrierId,
                                 text = a.Name
                             };
            return Json(carrier);
        }
        public IActionResult CreateCustomerOrderStatusList()
        {
            var orderStatus = from a in _context.CustomerOrderStatuses
                             select new
                             {
                                 value = a.CustomerOrderStatusId,
                                 text = a.Description
                             };
            return Json(orderStatus);
        }
        public IActionResult CreateProductName()
        {
            var ProductList = from a in _context.ProductMasters
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             join c in _context.ProductRegulatorCans on a.ProductId equals c.ProductId
                             where a.LobCode.Trim() == "CAN"  && a.ActiveFlag == true//&& b.SupplierId == currentSupplierId
                             orderby c.Description
                             select new
                             {
                                 label = c.ProductId,
                                 value = c.Sku + " - " +c.Description,
                                 supplier =  b.SupplierId

                             };
            return Json(ProductList);
        }
        public IActionResult CreateProductSkuList()
        {
            var sku = from a in _context.ProductRegulatorCans
                             orderby a.Description
                             select new
                             {
                                 text = a.Sku,
                                 value = a.Description

                             };
            return Json(sku);
        }
        public IActionResult CreateStockQtyList()
        {
            var stkQty = from a in _context.ProductMasters
                             join d in _context.Inventories on a.ProductId equals d.ProductId
                         join c in _context.ProductRegulatorLiqs on a.ProductId equals c.ProductId
                         where d.InventoryLocationId == 1
                             select new
                             {
                                 text = d.Count,
                                 value = c.Description

                             };
            return Json(stkQty);
        }
    }
}


