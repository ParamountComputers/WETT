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
    public class FulfillSalesController : Controller
    {
        public static Boolean pending;
        public static Boolean fulfilled;
        private DateTime searchDate;
        private readonly WETT_DBContext _context;
        public static long CurrentHeaderId=-1;

        public FulfillSalesController(WETT_DBContext context)
        {
            _context = context;
        }
       public async Task<IActionResult> Index()
        {
        pending = false;
        fulfilled = false;
        /*
         select * 
            from	[Customer Order] a,
                    [Customer] b,
                    [Customer Order Status] c,
                    [Carrier] d
            where	a.[Customer Id] = b.[Customer Id]
              and	a.[Customer Order Status Id] = c.[Customer Order Status Id]
              and   a.[Carrier Id] = d.[Carrier Id]

         */
        var result = from a in _context.CustomerOrders
                         join b in _context.Customers on a.CustomerId equals b.CustomerId
                         join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
                         join d in _context.Carriers on a.CarrierId equals d.CarrierId
                         select new FulfillSalesViewModel
                         {
                             CustomerOrderID = a.CustomerOrderId,
                             OrderDate = a.DateOrdered,
                             DelveryDate = a.DeliveryReqDate,
                             OrderNumber = a.OrderNumber,
                             Customer = b.Name,
                             City = b.City,
                             CarrierID = d.CarrierId,
                             CarrierDesc = d.Name,
                             Instructions = a.SpecialInstructions,
                             Status = c.Description


                         };
            return View(result);
        }
     
        public JsonResult GetAllHdr(JqGridViewModel request)
        {
           // var wETT_DBContext = _context.CustomerOrders;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var FulfillSalesData = from a in _context.CustomerOrders
                                   join b in _context.Customers on a.CustomerId equals b.CustomerId
                                   join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
                                   join d in _context.Carriers on a.CarrierId equals d.CarrierId
                                   select new FulfillSalesViewModel
                                      {
                                       CustomerOrderID = a.CustomerOrderId,
                                       OrderDate = a.DateOrdered,
                                       DelveryDate = a.DeliveryReqDate,
                                       OrderNumber = a.OrderNumber,
                                       Customer = b.Name,
                                       City = b.City,
                                       CarrierID = d.CarrierId,
                                       CarrierDesc = d.Name,
                                       Instructions = a.SpecialInstructions,
                                       Status = c.Description

                                   };
            var FulfillSalesDataTemp = FulfillSalesData;


            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "OrderDate":
                            FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(rule.data)));
                            searchDate = DateTime.Parse(rule.data);
                            break;
                        case "DelveryDate":
                            FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.DelveryDate.Equals(DateTime.Parse(rule.data)));
                            searchDate = DateTime.Parse(rule.data);
                            break;
                       // case "Supplier":
                      //      FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.ProductName.Contains(rule.data));
                      //      break;
                        case "Pending":
                            if (pending == false)
                            {
                                pending = true;
                            }
                            else
                            {
                                pending = false;
                            }
                            if (fulfilled == false && pending == true)
                            {
                                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.Status.Contains(rule.data));
                            }
                            else if (pending == false && fulfilled == true)
                            {
                                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.Status.Contains("Fulfilled"));
                            }
                            else
                            {
                                FulfillSalesData = FulfillSalesDataTemp;
                            }
                            break;
                        case "Fulfilled":
                            if (fulfilled == false)
                            {
                                fulfilled = true;
                            }
                            else
                            {
                                fulfilled = false;
                            }
                            if (pending == false && fulfilled == true)
                            {
                                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.Status.Contains(rule.data));
                            }
                            else if(pending ==true && fulfilled == false)
                            {
                                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.Status.Contains("Pending"));
                            }
                            else
                            {
                                FulfillSalesData = FulfillSalesDataTemp;
                            }
                            break;
                    /*    case "locationsDropdown":
                            if (rule.data.Contains("-1"))
                            {
                                break; 
                            }
                            else
                            {
                                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Where(w => w.InventoryLocationId.ToString().Contains(rule.data));
                                break;
                            }
                    */

                    }
                }

            int totalRecords = FulfillSalesData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.OrderByDescending(t => t.OrderDate);
                FulfillSalesData = FulfillSalesData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.OrderBy(t => t.OrderDate);
                FulfillSalesData = (IQueryable<FulfillSalesViewModel>)FulfillSalesData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = FulfillSalesData
            };

            return Json(jsonData);
        }

        public JsonResult GetAllDtls(JqGridViewModel request)
        {
          //  var wETT_DBContext = _context.Suppliers;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var AllFulfillSalesDtlsData =  from a in _context.CustomerOrders
                                        join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
                                        join c in _context.Products on b.ProductId equals c.ProductId
                                   select new FulfillSalesDtlsViewModel
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

            var FulfillSalesDtlsData = AllFulfillSalesDtlsData;

            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "Product":
                            FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.Where(w => w.ProductDesc.Equals(DateTime.Parse(rule.data)));
                            searchDate = DateTime.Parse(rule.data);
                            break;



                    }
                }
            if (CurrentHeaderId != -1)
            {
                //this is the type of transaction id
                FulfillSalesDtlsData = FulfillSalesDtlsData.Where(w => w.CustomerOrderID == CurrentHeaderId);

            }
            else
            {
                //this is to hide all transactons by type
                FulfillSalesDtlsData = FulfillSalesDtlsData.Where(w => w.CustomerOrderID == CurrentHeaderId);
            }


            int totalRecords = FulfillSalesDtlsData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.OrderByDescending(t => t.ProductDesc);
                FulfillSalesDtlsData = FulfillSalesDtlsData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.OrderBy(t => t.ProductDesc);
                FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = FulfillSalesDtlsData
            };

            return Json(jsonData);
        }
        public IActionResult CreateDetails(long data)
        {
            CurrentHeaderId = data;
            return Json(true);
       }
            public JsonResult Add(FulfillSalesViewModel p)
        {
            //InventoryTx s = new InventoryTx
            //{
            //    Date = searchDate,
            //    Comments = p.Comments,

            //};
            //_context.InventoryTxes.Add(s);
            //_context.SaveChanges();

            //InventoryTxDetail r = new InventoryTxDetail
            //{
            //    InventoryTxId=s.InventoryTxId,
            //    ToInventoryLocationId = p.InventoryLocationId,
            //    ProductId = p.ProductId,
            //    Amount = p.Amount
            //};
            //_context.InventoryTxDetails.Add(r);
            //_context.SaveChanges();
            return Json(true);

        }
        public JsonResult Update(FulfillSalesViewModel p)
        {


            //InventoryTxDetail r = _context.InventoryTxDetails.Single(a => a.InventoryTxDetailId == p.InventoryTxDetailId);
            //r.ToInventoryLocationId = p.InventoryLocationId;
            //r.ProductId = p.ProductId;
            //r.Amount = p.Amount;
            //_context.SaveChanges();
            return Json(true);
        }
        public JsonResult UpdateDtls(FulfillSalesDtlsViewModel p)
        {


            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(a => a.CustomerOrderDetailId == p.CustomerOrderDtlsID);
            r.QtyFulfilled = p.QtyFulfilled;
            _context.SaveChanges();
            return Json(true);
        }
        
        public JsonResult DeleteDtls(long id)
        {
            //InventoryTxDetail r = _context.InventoryTxDetails.Single(e => e.InventoryTxDetailId == id);
            //_context.InventoryTxDetails.Remove(r);
            //_context.SaveChanges();


            return Json(true);
        }
        public JsonResult Delete(long id)
        {
            //InventoryTxDetail r = _context.InventoryTxDetails.Single(e => e.InventoryTxDetailId == id);
            //_context.InventoryTxDetails.Remove(r);
            //_context.SaveChanges();


            return Json(true);
        }


        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from a in _context.Products
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             select new
                             {
                                 text = a.Sku,
                                 value = b.Name

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductName()
        {
            var invAdjData = from a in _context.Products
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             select new
                             {
                                 text = a.Description,
                                 value = b.Name,
                                 id = a.ProductId

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
        public IActionResult CreateLocationsList()
        {
            var invAdjData = from a in _context.InventoryLocations
                             select new
                             {
                                 value = a.InventoryLocationId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
        public IActionResult CreateSupplierList()
        {
            var invAdjData = from a in _context.Suppliers.Where(a => a.ActiveFlag == "Y")
                             select new
                             {
                                 value = a.SupplierId,
                                 text = a.Name
                             };
            return Json(invAdjData);
        }
    }
}
