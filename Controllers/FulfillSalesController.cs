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
        private static string orderSearchDate;
        private static string deliverySearchDate;
        private static long carrierId;
        private readonly WETT_DBContext _context;
        public static long CurrentHeaderId = -1;

        public FulfillSalesController(WETT_DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            pending = false;
            fulfilled = false;
            orderSearchDate = null;
            deliverySearchDate = null;
            carrierId = -1;

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
            //var result = from a in _context.CustomerOrders
            //             join b in _context.Customers on a.CustomerId equals b.CustomerId
            //             join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
            //             join d in _context.Carriers on a.CarrierId equals d.CarrierId
            //             select new FulfillSalesViewModel
            //             {
            //                 CustomerOrderID = a.CustomerOrderId,
            //                 OrderDate = a.DateOrdered,
            //                 DelveryDate = a.DeliveryReqDate,
            //                 OrderNumber = a.OrderNumber,
            //                 Customer = b.Name,
            //                 City = b.City,
            //                 CarrierID = d.CarrierId,
            //                 CarrierDesc = d.Name,
            //                 Instructions = a.SpecialInstructions,
            //                 Status = c.Description


            //             };



            var result = _context.SpGetFulfillSalesHdrs.FromSqlRaw("EXECUTE [dbo].[sp_getFulfillSalesHdr]").ToList();

            return View(result);




        }

        public JsonResult GetAllHdr(JqGridViewModel request)
        {
            // var wETT_DBContext = _context.CustomerOrders;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            //var FulfillSalesData = from a in _context.CustomerOrders
            //                       join b in _context.Customers on a.CustomerId equals b.CustomerId
            //                       join c in _context.CustomerOrderStatuses on a.CustomerOrderStatusId equals c.CustomerOrderStatusId
            //                       join d in _context.Carriers on a.CarrierId equals d.CarrierId
            //                       select new FulfillSalesViewModel
            //                          {
            //                           CustomerOrderID = a.CustomerOrderId,
            //                           OrderDate = a.DateOrdered,
            //                           DelveryDate = a.DeliveryReqDate,
            //                           OrderNumber = a.OrderNumber,
            //                           Customer = b.Name,
            //                           City = b.City,
            //                           CarrierID = d.CarrierId,
            //                           CarrierDesc = d.Name,
            //                           Instructions = a.SpecialInstructions,
            //                           Status = c.Description

            //                       };

            var FulfillSalesData = _context.SpGetFulfillSalesHdrs.FromSqlRaw("EXECUTE [dbo].[sp_getFulfillSalesHdr]").ToList();

            var FulfillSalesDataTemp = FulfillSalesData;


            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "OrderDate":
                            FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(rule.data))).ToList();
                            orderSearchDate = rule.data;
                            if (carrierId != -1)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.CarrierID.Equals(carrierId)).ToList();
                            }
                            if (fulfilled == false && pending == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Pending")).ToList();
                            }
                            else if (pending == false && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled")).ToList();
                            }
                            else if (pending == true && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled") || w.Status.Contains("Pending")).ToList();
                            }
                            if (deliverySearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(deliverySearchDate))).ToList();
                            }
                            break;
                        case "DeliveryDate":
                            FulfillSalesData = FulfillSalesData.Where(w => w.DelveryDate.Equals(DateTime.Parse(rule.data))).ToList();
                            deliverySearchDate = rule.data;
                            if (carrierId != -1)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.CarrierID.Equals(carrierId)).ToList();
                            }
                            if (fulfilled == false && pending == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Pending")).ToList();
                            }
                            else if (pending == false && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled")).ToList();
                            }
                            else if (pending == true && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled") || w.Status.Contains("Pending")).ToList();
                            }
                            if (orderSearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(orderSearchDate))).ToList();
                            }
                            break;
                        case "Carrier":
                            carrierId = (long)Convert.ToDouble(rule.data);
                            if (carrierId != -1)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.CarrierID.Equals(carrierId)).ToList();
                            }
                            else
                            {
                                FulfillSalesData = FulfillSalesDataTemp;
                            }
                            if (fulfilled == false && pending == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Pending")).ToList();
                            }
                            else if (pending == false && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled")).ToList();
                            }
                            else if (pending == true && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled") || w.Status.Contains("Pending")).ToList();
                            }
                            if (orderSearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(orderSearchDate))).ToList();
                            }
                            if (deliverySearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(deliverySearchDate))).ToList();
                            }

                            break;
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
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains(rule.data)).ToList();
                            }
                            else if (pending == false && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled")).ToList();
                            }
                            else if (pending == true && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled") || w.Status.Contains("Pending")).ToList();
                            }
                            else
                            {
                                FulfillSalesData = FulfillSalesDataTemp;
                            }
                            if (carrierId != -1)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.CarrierID.Equals(carrierId)).ToList();
                            }
                            if (orderSearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(orderSearchDate))).ToList();
                            }
                            if (deliverySearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(deliverySearchDate))).ToList();
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
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains(rule.data)).ToList();
                            }
                            else if (pending == true && fulfilled == false)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Pending")).ToList();
                            }
                            else if (pending == true && fulfilled == true)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.Status.Contains("Fulfilled") || w.Status.Contains("Pending")).ToList();
                            }
                            else
                            {
                                FulfillSalesData = FulfillSalesDataTemp;
                            }
                            if (carrierId != -1)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.CarrierID.Equals(carrierId)).ToList();
                            }
                            if (orderSearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(orderSearchDate))).ToList();
                            }
                            if (deliverySearchDate != null)
                            {
                                FulfillSalesData = FulfillSalesData.Where(w => w.OrderDate.Equals(DateTime.Parse(deliverySearchDate))).ToList();
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

            ////Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            //if (request.sord.ToUpper() == "DESC")
            //{
            //    FulfillSalesData = (List<SpGetFulfillSalesHdr>)(IQueryable<SpGetFulfillSalesHdr>)FulfillSalesData.OrderByDescending(t => t.OrderDate);
            //    FulfillSalesData = (List<SpGetFulfillSalesHdr>)FulfillSalesData.Skip(currentPageIndex * request.rows).Take(request.rows);
            //}
            //else
            //{
            //    FulfillSalesData = (List<SpGetFulfillSalesHdr>)(IQueryable<SpGetFulfillSalesHdr>)FulfillSalesData.OrderBy(t => t.OrderDate);
            //    FulfillSalesData = (List<SpGetFulfillSalesHdr>)(IQueryable<SpGetFulfillSalesHdr>)FulfillSalesData.Skip(currentPageIndex * request.rows).Take(request.rows);
            //}
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = FulfillSalesData
            };
            CurrentHeaderId = -1;
            return Json(jsonData);
        }

        public JsonResult GetAllDtls(JqGridViewModel request)
        {
            //  var wETT_DBContext = _context.Suppliers;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            //var AllFulfillSalesDtlsData = from a in _context.CustomerOrders
            //                              join b in _context.CustomerOrderDetails on a.CustomerOrderId equals b.CustomerOrderId
            //                              join c in _context.Products on b.ProductId equals c.ProductId
            //                              select new FulfillSalesDtlsViewModel
            //                              {
            //                                  CustomerOrderDtlsID = b.CustomerOrderDetailId,
            //                                  CustomerOrderID = a.CustomerOrderId,
            //                                  ProductID = c.ProductId,
            //                                  ProductSku = c.Sku,
            //                                  ProductDesc = c.Description,
            //                                  StockQty = 0,
            //                                  QtyOrdered = b.QtyOrdered,
            //                                  QtyFulfilled = b.QtyFulfilled,
            //                                  Notes = b.Notes
            //                              };

            var FulfillSalesDtlsData = _context.SpGetFulfillSalesDtls.FromSqlRaw("EXECUTE [dbo].[sp_getFulfillSalesDtls] @custOdrId =" + CurrentHeaderId).ToList();

            //var FulfillSalesDtlsData = AllFulfillSalesDtlsData;

            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            //if (issearch)
            //    foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
            //    {
            //        switch (rule.field)
            //        {
            //            case "Product":
            //                FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.Where(w => w.ProductDesc.Equals(DateTime.Parse(rule.data)));
            //               // searchDate = DateTime.Parse(rule.data);
            //                break;



            //        }
            //    }
            if (CurrentHeaderId == -1)
            {
                //this is the type of transaction id
                FulfillSalesDtlsData = new List<SpGetFulfillSalesDtls>();
                //(List<SpGetFulfillSalesDtls>)FulfillSalesDtlsData.Where(w => w.CustomerOrderID == CurrentHeaderId);
            }
            else
            {
                //this is to hide all transactons by type
                //FulfillSalesDtlsData = (List<SpGetFulfillSalesDtls>)(IQueryable<SpGetFulfillSalesDtls>)FulfillSalesDtlsData.Where(w => w.CustomerOrderID == CurrentHeaderId); //FulfillSalesDtlsData.Where(w => w.CustomerOrderID == CurrentHeaderId);
            }


            int totalRecords = FulfillSalesDtlsData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            //if (request.sord.ToUpper() == "DESC")
            //{
            //    FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.OrderByDescending(t => t.ProductDesc);
            //    FulfillSalesDtlsData = FulfillSalesDtlsData.Skip(currentPageIndex * request.rows).Take(request.rows);
            //}
            //else
            //{
            //    FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.OrderBy(t => t.ProductDesc);
            //    FulfillSalesDtlsData = (IQueryable<FulfillSalesDtlsViewModel>)FulfillSalesDtlsData.Skip(currentPageIndex * request.rows).Take(request.rows);
            //}
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
        public JsonResult UpdateHdr(FulfillSalesViewModel p)
        {


            CustomerOrder r = _context.CustomerOrders.Single(a => a.CustomerOrderId == p.CustomerOrderID);
            r.CarrierId = p.CarrierID;
            _context.SaveChanges();
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
            CustomerOrderDetail r = _context.CustomerOrderDetails.Single(e => e.CustomerOrderDetailId == id);
            _context.CustomerOrderDetails.Remove(r);
            _context.SaveChanges();


            return Json(true);
        }
        public JsonResult Delete(long id)
        {
            CustomerOrder r = _context.CustomerOrders.Single(e => e.CustomerOrderId == id);
            _context.CustomerOrders.Remove(r);
            _context.SaveChanges();


            return Json(true);
        }


        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from a in _context.ProductMasters
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             join c in _context.ProductRegulatorLiqs on a.ProductId equals c.ProductId
                             select new
                             {
                                 text = c.Sku,
                                 value = b.Name

                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductName()
        {
            var invAdjData = from a in _context.ProductMasters
                             join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                             join c in _context.ProductRegulatorLiqs on a.ProductId equals c.ProductId
                             orderby c.Description
                             select new
                             {
                                 text = c.Description,
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
        public IActionResult CreateOrderSourceList()
        {
            var invAdjData = from a in _context.OrderSources
                             select new
                             {
                                 value = a.OrderSourceId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
    }
}
