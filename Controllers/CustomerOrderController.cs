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
        public static long currentCustomer;
        public static string currentLob;
        public static string currentOrderNumber;
        public static DateTime currentDateOrdered;
        public static long currentCustomerOrderStatus;
        public static string currentDriver;
        public static string currentDsSlipNumber;
        public static DateTime currentDeliveryReqDate;
        public static string currentSpecialInstructions;
        public static long currentCarrier;
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
                CurrentCustomerOrderId = (long)Convert.ToDouble(CustomerOrderID);
            }
            else
            {
                CurrentCustomerOrderId = -1;
            }
        

            var result = from a in _context.CustomerOrderDetails
                         join b in _context.CustomerOrders on a.CustomerOrderId equals b.CustomerOrderId
                                          join c in _context.ProductMasters on a.ProductId equals c.ProductId
                                          join d in _context.ProductRegulatorLiqs on a.ProductId equals d.ProductId
                                          where a.CustomerOrderId == CurrentCustomerOrderId && c.LobCode.Trim() == "LIQ"
                         select new CustomerOrderViewModel
                                          {
                             CustomerOrderDetailId = a.CustomerOrderDetailId,
                                              CustomerOrderID = b.CustomerOrderId,
                                              ProductID = c.ProductId,
                                              ProductSku = d.Sku,
                                              ProductDesc = d.Description,
                                              StockQty = 0,
                                              QtyOrdered = a.QtyOrdered,
                                              Notes = a.Notes
                                          };
            return View(result);
        }


        public JsonResult GetAll(JqGridViewModel request)
        {
            var AllCustomerOrderData = from a in _context.CustomerOrderDetails
                                       join b in _context.CustomerOrders on a.CustomerOrderId equals b.CustomerOrderId
                                       join c in _context.ProductMasters on a.ProductId equals c.ProductId
                                       join d in _context.ProductRegulatorLiqs on a.ProductId equals d.ProductId
                                       where a.CustomerOrderId == CurrentCustomerOrderId && c.LobCode.Trim() == "LIQ"
                                       select new CustomerOrderViewModel
                                          {
                                           CustomerOrderDetailId = a.CustomerOrderDetailId,
                                              CustomerOrderID = b.CustomerOrderId,
                                              ProductID = c.ProductId,
                                              ProductSku = d.Sku,
                                              ProductDesc = d.Description,
                                              StockQty = 0,
                                              QtyOrdered = a.QtyOrdered,
                                              Notes = a.Notes
                                          };
            var CustomerOrderData = AllCustomerOrderData;
            if (CurrentCustomerOrderId != -1)
            {
                CustomerOrderData = CustomerOrderData.Where(w => w.CustomerOrderID == CurrentCustomerOrderId);

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
           // ProductMaster s = _context.ProductMasters.Single(a => a.ProductId == p.ProductID);
            ProductRegulatorLiq s = _context.ProductRegulatorLiqs.Single(a => a.Description == p.ProductDesc);
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(a => a.CustomerOrderDetailId == p.CustomerOrderDetailId);
                r.ProductId = s.ProductId;
                r.QtyOrdered = p.QtyOrdered;
                r.Notes = p.Notes;
                r.UpdateTimestamp= DateTime.Now;
                r.UpdateUserid = User.Identity.Name;
            _context.SaveChanges();
            return Json(true);
        }

        public JsonResult Add(CustomerOrderViewModel p)
        {
            if (CurrentCustomerOrderId == -1)
            {

                CustomerOrder s = new CustomerOrder
                {
                    CustomerId = currentCustomer,
                    OrderNumber = currentOrderNumber,
                    DateOrdered = currentDateOrdered,
                    CustomerOrderStatusId = currentCustomerOrderStatus,
                    CarrierId = currentCarrier,
                    Driver = currentDriver,
                    DsSlipNumber = currentDsSlipNumber,
                    DeliveryReqDate = currentDeliveryReqDate,
                    SpecialInstructions = currentSpecialInstructions,
                    //hard coded for now
                    LobCode = "LIQ",
                    OrderSourceId = 1,
                    InsertTimestamp = DateTime.Now,
                    InsertUserId = User.Identity.Name,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserId = User.Identity.Name,

                };

                _context.CustomerOrders.Add(s);
                _context.SaveChanges();
                CurrentCustomerOrderId = s.CustomerOrderId;
            }
            else
            {
                CustomerOrder s = _context.CustomerOrders.Single(a => a.CustomerOrderId == CurrentCustomerOrderId);
                s.CustomerId = currentCustomer;
                s.OrderNumber = currentOrderNumber;
                s.DateOrdered = currentDateOrdered;
                s.CarrierId = currentCarrier;
                s.CustomerOrderStatusId = currentCustomerOrderStatus;
                s.Driver = currentDriver;
                s.DsSlipNumber = currentDsSlipNumber;
                s.DeliveryReqDate = currentDeliveryReqDate;
                s.SpecialInstructions = currentSpecialInstructions;
                s.UpdateTimestamp = DateTime.Now;
                s.UpdateUserId = User.Identity.Name;
                _context.SaveChanges();
            }
            //ProductMaster c = _context.ProductMasters.Single(a => a.Description == p.ProductDesc);
            //ProductMaster c = _context.ProductMasters.Single(a => a.ProductId == p.ProductID);
            ProductRegulatorLiq c = _context.ProductRegulatorLiqs.Single(a => a.Description == p.ProductDesc);
            CustomerOrderDetail r = new CustomerOrderDetail
            {
                CustomerOrderId = CurrentCustomerOrderId,
                ProductId = c.ProductId,
                QtyOrdered = p.QtyOrdered,
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
        public JsonResult Delete(long id)
        {
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(e => e.CustomerOrderDetailId == id);
            _context.CustomerOrderDetails.Remove(r);
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
                    dateOrdered = r.DateOrdered.ToShortDateString(),
                    customerOrderStatus = r.CustomerOrderStatusId,
                    carrier = r.CarrierId,
                    driver = r.Driver,
                    dsSlipNumber = r.DsSlipNumber,
                    deliveryReqDate = r.DeliveryReqDate.ToShortDateString(),
                    specialInstructions = r.SpecialInstructions,
                    lob = r.LobCode
                };
                return Json(headerInfo);
            }
            return Json(null);
        }
        public IActionResult CreateHeader(string data)
        {
            var li = data.Split("/");
            currentCustomer = (long)Convert.ToDouble(li[0]);
            currentOrderNumber = li[1];
            currentDateOrdered = DateTime.Parse(li[2]);
            currentCustomerOrderStatus = (long)Convert.ToDouble(li[3]);
            currentDriver = li[4];
            currentDsSlipNumber = li[5];
            currentDeliveryReqDate = DateTime.Parse(li[6]);
            currentSpecialInstructions = li[7];
            currentCarrier = (long)Convert.ToDouble(li[8]);
            //currentLob = li[8];
            return Json(true);
        }


        public IActionResult CreateCustomerList()
        {
            var invAdjData = from a in _context.Customers
                             orderby a.Name
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
                             orderby a.Name
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
                             join c in _context.ProductRegulatorLiqs on a.ProductId equals c.ProductId
                             where a.LobCode.Trim() == "LIQ" //&& b.SupplierId == currentSupplierId
                             orderby c.Description
                             select new
                             {
                                 label = a.ProductId,
                                 value = c.Description


                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from b in _context.ProductRegulatorLiqs
                             orderby b.Description
                             select new
                             {
                                 text = b.Sku,
                                 value = b.Description

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateStockQtyList()
        {
            var invAdjData = from a in _context.ProductMasters
                             join c in _context.ProductRegulatorLiqs on a.ProductId equals c.ProductId
                             join d in _context.Inventories on a.ProductId equals d.ProductId
                             where d.InventoryLocationId == 1
                             select new
                             {
                                 text = d.Count,
                                 value = c.Description

                             };
            return Json(invAdjData);
        }
    }
}

