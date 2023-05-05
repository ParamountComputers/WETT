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
        public static long currentCustomer;
        public static string currentOrderNumber;
        public static DateTime currentDateOrdered;
        public static DateTime currentReceivedDate;
        public static long currentCustomerOrderStatus;
        public static string currentSpecialInstructions;
        public static long currentCarrier;
        public static long CurrentCustomerOrderId;
        public static long currentSupplierId;
        public static IQueryable<CustomerList> customerList;
        private readonly WETT_DBContext _context;

        public CannabisOrderEntryController(WETT_DBContext context)
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
                         where a.CustomerOrderId == CurrentCustomerOrderId && a.LobCode.Trim() == "CAN" //&& c.SupplierId == currentSupplierId
                         select new CustomerOrderViewModel
                         {
                             CustomerOrderDtlsID = b.CustomerOrderDetailId,
                             CustomerOrderID = a.CustomerOrderId,
                             ProductID = c.ProductId,
                             ProductSku = d.Sku,
                             ProductDesc = c.Description,
                             QtyOrdered = b.QtyOrdered,
                             //QtyFulfilled = b.QtyFulfilled,
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
                                       where a.CustomerOrderId == CurrentCustomerOrderId && a.LobCode.Trim() == "CAN" //&& c.SupplierId == currentSupplierId
                                       select new CustomerOrderViewModel
                                       {
                                           CustomerOrderDtlsID = b.CustomerOrderDetailId,
                                           CustomerOrderID = a.CustomerOrderId,
                                           ProductID = c.ProductId,
                                           ProductSku = d.Sku,
                                           ProductDesc = c.Description,
                                           QtyOrdered = b.QtyOrdered,
                                           //QtyFulfilled= b.QtyFulfilled,
                                           Notes = b.Notes
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
            ProductMaster s = _context.ProductMasters.Single(a => a.Description == p.ProductDesc);
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(a => a.CustomerOrderDetailId == p.CustomerOrderDtlsID);
            r.ProductId = s.ProductId;
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
            if (CurrentCustomerOrderId == -1)
            {

                CustomerOrder s = new CustomerOrder
                {
                    CustomerId = currentCustomer,
                    OrderNumber = currentOrderNumber,
                    DateOrdered = currentDateOrdered,
                    //DateReceived = currentReceivedDate,
                    CustomerOrderStatusId = currentCustomerOrderStatus,
                    //CarrierId = currentCarrier,
                    SpecialInstructions = currentSpecialInstructions,
                    //hard coded for now
                    LobCode = "CAN",
                    OrderSourceId = 1,
                    CarrierId = 1,
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
                //s.DateReceived = currentReceivedDate;
               //s.CarrierId = currentCarrier;
                s.CustomerOrderStatusId = currentCustomerOrderStatus;
                s.SpecialInstructions = currentSpecialInstructions;
                s.UpdateTimestamp = DateTime.Now;
                s.UpdateUserId = User.Identity.Name;
                _context.SaveChanges();
            }
            ProductMaster c = _context.ProductMasters.Single(a => a.Description == p.ProductDesc);
            CustomerOrderDetail r = new CustomerOrderDetail
            {
                CustomerOrderId = CurrentCustomerOrderId,
                ProductId = c.ProductId,
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
                };
                return Json(headerInfo);
            }
            return Json(null);
        }
        public IActionResult CreateHeader(string data)
        {
            var li = data.Split("/");
            //CustomerList r = customerList.Single(x => x.text == li[0]);
            //currentCustomer = r.value;
            currentCustomerOrderStatus = (long)Convert.ToDouble(li[3]);
            currentOrderNumber = li[1];
            if (li[2] != ""){
                currentDateOrdered = DateTime.Parse(li[2]);
            }
            if (li[6] != "")
            {
                currentReceivedDate = DateTime.Parse(li[6]);
            }
            currentSpecialInstructions = li[4];
            currentSupplierId = (long)Convert.ToDouble(li[5]);
            var mllb = li[0].Split(" ");
            Customer r = _context.Customers.Single(x => x.MbllCustomerNo == (long)Convert.ToDouble(mllb[0]));
            currentCustomer = r.CustomerId;
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
            customerList = from a in _context.Customers
                               // orderby a.Name
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
            var invAdjData = from a in _context.ProductMasters
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             where a.LobCode.Trim() == "CAN" //&& b.SupplierId == currentSupplierId
                             orderby a.Description
                             select new
                             {
                                 label = a.ProductId,
                                 value = a.Description,
                                 supplier =  b.SupplierId

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductSkuList()
        {
            var sku = from a in _context.ProductMasters
                             join b in _context.ProductRegulatorCan on a.ProductId equals b.ProductId
                             orderby a.Description
                             select new
                             {
                                 text = b.Sku,
                                 value = a.Description

                             };
            return Json(sku);
        }
        public IActionResult CreateStockQtyList()
        {
            var stkQty = from a in _context.ProductMasters
                             join d in _context.Inventories on a.ProductId equals d.ProductId
                             where d.InventoryLocationId == 1
                             select new
                             {
                                 text = d.Count,
                                 value = a.Description

                             };
            return Json(stkQty);
        }
    }
}


