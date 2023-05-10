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
    public class InventoryController : Controller
    {
        private readonly WETT_DBContext _context;

        public InventoryController(WETT_DBContext context)
        {
            _context = context;
        }

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

            //var products = from s in _context.Products
            //               select s;

            var result= from a in _context.Inventories
                         join b in _context.ProductMasters on a.ProductId equals b.ProductId
                         join c in _context.InventoryLocations on a.InventoryLocationId equals c.InventoryLocationId
                         join d in _context.Suppliers on b.SupplierId equals d.SupplierId
                         join e in _context.ProductRegulatorLiqs on a.ProductId equals e.ProductId
                        select new InventoryViewModel
                         {
                             ProductId = a.ProductId,
                             ProductName = e.Description,
                             ProductSku = e.Sku,
                             Supplier = d.Name,
                             InvLocationId = a.InventoryLocationId,
                             InvLocationName = c.Description,
                             InvCount = a.Count,
                             Date = a.Date.ToShortDateString()
                         };


            //return View(result);

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.ProductName.Contains(searchString)
                                       || s.ProductSku.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "sku":
                    result = result.OrderBy(s => s.ProductSku);
                    break;
                case "Description":
                    result = result.OrderBy(s => s.ProductName);
                    break;
                case "Supplier":
                    result = result.OrderBy(s => s.Supplier);
                    break;
                default:
                    result = result.OrderBy(s => s.ProductSku);
                    break;
            }

            //return View(await products.AsNoTracking().ToListAsync());
            int pageSize = 3;
            return View(await PaginatedList<InventoryViewModel>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));

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
        //public IActionResult Create()
        //{
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name");
        //    return View();
        //}

        //// POST: Products/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ProductId,SupplierId,Sku,Description,SingleWeight,ContainerWeight,CaseWeight,PackSize,HlSingle,HlContainer,HlCase")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
        //    return View(product);
        //}

        //// GET: Products/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
        //    return View(product);
        //}

        //// POST: Products/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("ProductId,SupplierId,Sku,Description,SingleWeight,ContainerWeight,CaseWeight,PackSize,HlSingle,HlContainer,HlCase")] Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.ProductId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
        //    return View(product);
        //}

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
            var wETT_DBContext = _context.ProductMasters;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var productData = from a in _context.Inventories
                              join b in _context.ProductMasters on a.ProductId equals b.ProductId
                              join c in _context.InventoryLocations on a.InventoryLocationId equals c.InventoryLocationId
                              join d in _context.Suppliers on b.SupplierId equals d.SupplierId
                              join e in _context.ProductRegulatorLiqs on a.ProductId equals e.ProductId
                              select new InventoryViewModel
                              {
                                  ProductId = a.ProductId,
                                  ProductName = e.Description,
                                  ProductSku = e.Sku,
                                  Supplier = d.Name,
                                  InvLocationId = a.InventoryLocationId,
                                  InvLocationName = c.Description,
                                  InvCount = a.Count,
                                  Date = a.Date.ToShortDateString()
                              };


            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "name":
                            productData = (IQueryable<InventoryViewModel>)productData.Where(w => w.ProductName.Contains(rule.data));
                            break;
                    }
                }

            int totalRecords = productData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                productData = (IQueryable<InventoryViewModel>)productData.OrderByDescending(t => t.ProductName);
                productData = (IQueryable<InventoryViewModel>)productData.Skip(currentPageIndex * request.rows).Take(request.rows);
            }
            else
            {
                productData = (IQueryable<InventoryViewModel>)productData.OrderBy(t => t.ProductName);
                productData = (IQueryable<InventoryViewModel>)productData.Skip(currentPageIndex * request.rows).Take(request.rows);
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
        //public JsonResult Update(Product p)
        //{

        //    Product r = _context.Products.Single(e => e.ProductId == p.ProductId);
        //    r.SupplierId = p.SupplierId;
        //    r.Sku = p.Sku;
        //    r.Description = p.Description;
        //    r.SingleWeight = p.SingleWeight;
        //    r.ContainerWeight = p.ContainerWeight;
        //    r.CaseWeight = p.CaseWeight;
        //    r.PackSize = p.PackSize;
        //    r.HlSingle = p.HlSingle;
        //    r.HlContainer = p.HlContainer;
        //    r.HlCase = p.HlCase;


        //    _context.SaveChanges();


        //    return Json(true);
        //}

        //public JsonResult Delete(int id)
        //{
        //    Product r = _context.Products.Single(e => e.ProductId == id);
        //    _context.Products.Remove(r);
        //    _context.SaveChanges();


        //    return Json(true);
        //}

        //public JsonResult Add(Product p)
        //{


        //    _context.Products.Add(p);
        //    _context.SaveChanges();

        //    return Json(true);
        //}
        public IActionResult CreateList()
        {

            var li = from a in _context.ProductMasters
                     join b in _context.Suppliers on a.SupplierId equals b.SupplierId
                     join c in _context.ProductRegulatorLiqs on a.ProductId equals c.ProductId
                     where b.ActiveFlag == "Y"
                     select new
                     {
                         text = b.Name,
                         value = c.Description

                     };
            return Json(li);
        }
        public IActionResult CreateProductSkuList()
        {
            var invAdjData = from a in _context.ProductRegulatorLiqs
                             select new
                             {
                                 text = a.Sku,
                                 value = a.Description
                             };
            return Json(invAdjData);
        }
        public IActionResult CreateProductName()
        {
            var invAdjData = from a in _context.ProductMasters
                             join b in _context.ProductRegulatorLiqs on a.ProductId equals b.ProductId
                             select new
                             {
                                 value = a.SupplierId,
                                 text = b.Description
                             };
            return Json(invAdjData);
        }
    }
}
