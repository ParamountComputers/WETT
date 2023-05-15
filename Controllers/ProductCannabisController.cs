using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.SecurityNamespace;
using WETT.Data;
using WETT.Models;

namespace WETT.Controllers
{
    public class ProductCannabisController : Controller
    {
        private readonly WETT_DBContext _context;

        public ProductCannabisController(WETT_DBContext context)
        {
            _context = context;
        }

        // GET: Products
        // public async Task<IActionResult> Index()
        // {
        //     var wETT_DBContext = _context.Products.Include(p => p.Supplier);
        //     return View(await wETT_DBContext.ToListAsync());
        // }
        public async Task<IActionResult> Index()
        {
            var result = from a in _context.ProductMasters
                         join b in _context.ProductRegulatorCans on a.ProductId equals b.ProductId
                         where b.ActiveFlag == true
                         select new ProductCannabisViewModel
                         {
                             SupplierId = a.SupplierId,
                             ProductId = b.ProductId,
                             RegulatorCode = b.RegulatorCode,
                             ProvinceCode = b.ProvinceCode,
                             Sku = b.Sku,
                             Description = b.Description,
                             Description2 = b.Description2,

                         };
            return View(result);
        }
        public JsonResult GetAll(JqGridViewModel request)
        {
            //sets up grid with proper data
            var products = from a in _context.ProductMasters
                                join b in _context.ProductRegulatorCans on a.ProductId equals b.ProductId
                                where b.ActiveFlag == true
                                select new ProductCannabisViewModel
                                {
                                    SupplierId = a.SupplierId,
                                    ProductId = b.ProductId,
                                    RegulatorCode = b.RegulatorCode,
                                    ProvinceCode = b.ProvinceCode,
                                    Sku = b.Sku,
                                    Description = b.Description,
                                    Description2 = b.Description2,

                                };
            var productsData = products;
            int totalRecords = productsData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;
            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "name":
                            productsData = productsData.Where(w => w.Description.Contains(rule.data));
                            break;
                    }
                }

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                productsData = (IQueryable<ProductCannabisViewModel>)productsData.OrderByDescending(t => t.Description);
                productsData = productsData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                productsData = (IQueryable<ProductCannabisViewModel>)productsData.OrderBy(t => t.Description);
                productsData = (IQueryable<ProductCannabisViewModel>)productsData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = productsData
            };

            return Json(jsonData);
        }
        public JsonResult Update(ProductCannabisViewModel p)
        {
            //updates the Inventory details to new entries 
            ProductRegulatorCan s = _context.ProductRegulatorCans.Single(a => a.ProductId == p.ProductId);
            ProductMaster r = _context.ProductMasters.Single(a => a.ProductId == p.ProductId);
            s.ProductId = p.ProductId;
            s.Sku = p.Sku;
            s.Description = p.Description;
            s.Description2 = p.Description2;
            r.UpdateUserId = User.Identity.Name;
            r.UpdateTimestamp = DateTime.Now;
            _context.SaveChanges();
            return Json(true);
        }
        public JsonResult Add(ProductCannabisViewModel p)
        {

                ProductMaster s = new ProductMaster
                {
                    SupplierId = p.SupplierId,
                    LobCode = "CAN",
                    ActiveFlag = true,
                    InsertUserId = User.Identity.Name,
                    InsertTimestamp = DateTime.Now,
                    UpdateTimestamp = DateTime.Now,
                    UpdateUserId = User.Identity.Name,
                };
                _context.ProductMasters.Add(s);
                _context.SaveChanges();

            ProductRegulatorCan r = new ProductRegulatorCan
            {
                ProductId = s.ProductId,
                RegulatorCode = "MBLL",
                ProvinceCode = "MB",
                Sku = p.Sku,
                Description = p.Description,
                Description2 = p.Description2,
                ActiveFlag = true,
                InsertUserId = User.Identity.Name,


            };

            _context.ProductRegulatorCans.Add(r);
            _context.SaveChanges();

            return Json(true);
        }
        public JsonResult Delete(long id)
        {
            ProductRegulatorCan r = _context.ProductRegulatorCans.Single(e => e.ProductId == id);
            ProductMaster s = _context.ProductMasters.Single(e => e.ProductId == id);
            r.ActiveFlag = false;
            s.ActiveFlag = false;
            _context.SaveChanges();

            return Json(true);
        }

        public IActionResult CreateSupplierList()
        {

            var SupplierList = from a in _context.Suppliers 
                     where a.ActiveFlag == "Y"
                     select new
                     {
                         text = a.Name,
                         value = a.SupplierId

                     };
            return Json(SupplierList);
        }
        public IActionResult CreateProductSkuList()
        {
            var productSku = from a in _context.ProductRegulatorLiqs
                             select new
                             {
                                 text = a.Sku,
                                 value = a.Description
                             };
            return Json(productSku);
        }
        public IActionResult CreateProductName()
        {
            var productName = from  b in _context.ProductRegulatorCans 
                             select new
                             {
                                 value = b.ProductId,
                                 text = b.Description
                             };
            return Json(productName);
        }
        public IActionResult CreateLocationList()
        {
            var Locations = from a in _context.InventoryLocations
                             select new
                             {
                                 value = a.InventoryLocationId,
                                 text = a.Description
                             };
            return Json(Locations);
        }
        public IActionResult CreateReasonsList()
        {
            var Reasons = from a in _context.InventoryTxReasons
                             select new
                             {
                                 value = a.InventoryTxReasonId,
                                 text = a.Description
                             };
            return Json(Reasons);
        }
    }
}

