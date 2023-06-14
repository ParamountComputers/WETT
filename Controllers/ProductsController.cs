using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.SecurityNamespace;
using WETT.Data;
using WETT.Models;
using BindAttribute = Microsoft.AspNetCore.Mvc.BindAttribute;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;
using ValidateAntiForgeryTokenAttribute = Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute;

namespace WETT.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WETT_DBContext _context;

        public ProductsController(WETT_DBContext context)
        {
            _context = context;
        }

        // GET: Products
        // public async Task<IActionResult> Index()
        // {
        //     var wETT_DBContext = _context.Products.Include(p => p.Supplier);
        //     return View(await wETT_DBContext.ToListAsync());
        // }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DescriptionParm"] = String.IsNullOrEmpty(sortOrder) ? "Description" : "";
            ViewData["SkuParm"] = sortOrder == "Sku" ? "Sku" : "Sku";
            ViewData["SupplierParm"] = String.IsNullOrEmpty(sortOrder) ? "Supplier" : "";



            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var products = from a in _context.ProductMasters
                           join b in _context.ProductRegulatorLiqs on a.ProductId equals b.ProductId
                           join c in _context.Suppliers on a.SupplierId equals c.SupplierId
                           where b.ActiveFlag == true
                           select new ProductLiq
                           {
                              
                               ProductId = a.ProductId,
                               Description= b.Description,
                               SupplierId = a.SupplierId,
                               LobCode = a.LobCode,
                               Sku = b.Sku,
                               Description2 = b.Description2,
                               SingleWeight = b.SingleWeight,
                               ContainerWeight = b.ContainerWeight,
                               CaseWeight = b.CaseWeight,
                               PackSize = b.PackSize,
                               HlSingle = b.HlSingle,
                               HlContainer = b.HlContainer,
                               HlCase = b.HlCase
                           };
                            

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Description.Contains(searchString)
                                       || s.Sku.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "sku":
                    products = products.OrderBy(s => s.Sku);
                    break;
                case "Description":
                    products = products.OrderBy(s => s.Description);
                    break;
                //case "Supplier":
                  //  products = products.OrderBy(s => s.Supplier);
                    //break;
                default:
                    products = products.OrderBy(s => s.Sku);
                    break;
            }

            //return View(await products.AsNoTracking().ToListAsync());
            int pageSize = 3;
            return View(await PaginatedList<ProductLiq>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductMasters
                .Include(p => p.SupplierId)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,SupplierId,Sku,Description,SingleWeight,ContainerWeight,CaseWeight,PackSize,HlSingle,HlContainer,HlCase")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductMasters.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProductId,SupplierId,Sku,Description,SingleWeight,ContainerWeight,CaseWeight,PackSize,HlSingle,HlContainer,HlCase")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        /* public async Task<IActionResult> Delete(long? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var product = await _context.Products
                 .Include(p => p.Supplier)
                 .FirstOrDefaultAsync(m => m.ProductId == id);
             if (product == null)
             {
                 return NotFound();
             }

             return View(product);
         }
        */
        // POST: Products/Delete/5
        // [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        /*  public async Task<IActionResult> DeleteConfirmed(long id)
          {
              var product = await _context.Products.FindAsync(id);
              _context.Products.Remove(product);
              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
          }
        */
        private bool ProductExists(long id)
        {
            return _context.ProductMasters.Any(e => e.ProductId == id);
        }
        public JsonResult GetAll(JqGridViewModel request)
        {
           // var wETT_DBContext = _context.ProductRegulatorLiqs;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var productData =  from a in _context.ProductMasters
                                             join b in _context.ProductRegulatorLiqs on a.ProductId equals b.ProductId
                                             join c in _context.Suppliers on a.SupplierId equals c.SupplierId
                                             where b.ActiveFlag == true
                                             select new ProductLiq
                                             {

                                                 ProductId = a.ProductId,
                                                 Description = b.Description,
                                                 SupplierId = a.SupplierId,
                                                 LobCode = a.LobCode,
                                                 Sku = b.Sku,
                                                 Description2 = b.Description2,
                                                 SingleWeight = b.SingleWeight,
                                                 ContainerWeight = b.ContainerWeight,
                                                 CaseWeight = b.CaseWeight,
                                                 PackSize = b.PackSize,
                                                 HlSingle = b.HlSingle,
                                                 HlContainer = b.HlContainer,
                                                 HlCase = b.HlCase
                                             };


            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "name":
                            productData = (IQueryable<ProductLiq>)productData.Where(w => w.Description.Contains(rule.data));
                            break;
                    }
                }

            int totalRecords = productData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                productData = (IQueryable<ProductLiq>)productData.OrderByDescending(t => t.Description);
                productData = (IQueryable<ProductLiq>)productData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                productData = (IQueryable<ProductLiq>)productData.OrderBy(t => t.Description);
                productData = (IQueryable<ProductLiq>)productData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }

            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = productData
            };

            return Json(jsonData);
        }
        public JsonResult Update(ProductLiq p)
        {

            ProductRegulatorLiq r = _context.ProductRegulatorLiqs.Single(e => e.ProductId == p.ProductId);
            ProductMaster s = _context.ProductMasters.Single(a => a.ProductId == p.ProductId);
            s.SupplierId = p.SupplierId;
            r.Sku = p.Sku;
            r.Description = p.Description;
            r.SingleWeight = p.SingleWeight;
            r.ContainerWeight = p.ContainerWeight;
            r.CaseWeight = p.CaseWeight;
            r.PackSize = p.PackSize;
            r.HlSingle = p.HlSingle;
            r.HlContainer = p.HlContainer;
            r.HlCase = p.HlCase;
            s.UpdateUserId = User.Identity.Name;
            s.UpdateTimestamp = DateTime.Now;


            _context.SaveChanges();


            return Json(true);
        }

        public JsonResult Delete(int id)
        {
            ProductRegulatorLiq r = _context.ProductRegulatorLiqs.Single(e => e.ProductId == id);
            ProductMaster s = _context.ProductMasters.Single(e => e.ProductId == id);
            r.ActiveFlag = false;
            s.ActiveFlag = false;
            _context.SaveChanges();

            return Json(true);
        }

        public JsonResult Add(ProductLiq p)
        {
            ProductMaster s = new ProductMaster
            {
                SupplierId = p.SupplierId,
                LobCode = "LIQ",
                ActiveFlag = true,
                InsertUserId = User.Identity.Name,
                InsertTimestamp = DateTime.Now,
                UpdateTimestamp = DateTime.Now,
                UpdateUserId = User.Identity.Name,
        };
            _context.ProductMasters.Add(s);
            _context.SaveChanges();

            ProductRegulatorLiq r = new ProductRegulatorLiq
            {
                ProductId = s.ProductId,
                ContainerWeight= p.ContainerWeight,
                PackSize=p.PackSize,
                HlCase=p.HlCase,    
                HlContainer=p.HlContainer,
                HlSingle=p.HlSingle,
                CaseWeight= p.CaseWeight,
                RegulatorCode = "MBLL",
                ProvinceCode = "MB",
                Sku = p.Sku,
                SingleWeight= p.SingleWeight,
                Description = p.Description,
                Description2 = p.Description2,
                ActiveFlag = true,
                InsertUserId = User.Identity.Name,


            };

            _context.ProductRegulatorLiqs.Add(r);
            _context.SaveChanges();

            return Json(true);
        }
        [HttpGet]
        public IActionResult CreateList()
        {
            var li = from s in _context.Suppliers.Where(a => a.ActiveFlag == "Y")
                     where s.LobCode == "LIQ"
                     select new
                     {
                         text = s.Name,
                         value = s.SupplierId
                     };
            return Json(li);
        }
    }
}
