using Microsoft.AspNetCore.Mvc;
using WETT.Data;
using WETT.Models;
using System.Linq;
using System.Linq.Dynamic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WETT.Controllers
{
    public class SupplierController : Controller
    {
        private Supplier db = new Supplier();

        //ty add
        private readonly WETT_DBContext _context;
        public SupplierController(WETT_DBContext context)
        {
            _context = context;
        }


        //GET: Products
         public async Task<IActionResult> Index()
        {
            var wETT_DBContext = _context.Suppliers;
            return View(await wETT_DBContext.ToListAsync());
        }

        //end ty add


        // public IActionResult Index()
        // {

        //     return View();
        //}

        public JsonResult GetAll(JqGridViewModel request)
        {
            var wETT_DBContext = _context.Suppliers;
            // var supplierData = new SupplierViewModel().SuppliersDatabase;
            var supplierData = _context.Suppliers.ToList();


            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        case "name":
                            supplierData = supplierData.Where(w => w.Name.Contains(rule.data)).ToList();
                            break;
                    }
                }
            
            int totalRecords = supplierData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                supplierData = supplierData.OrderByDescending(t => t.Name).ToList();
                supplierData = supplierData.Skip(currentPageIndex * request.rows).Take(request.rows).ToList();
            }
            else
            {
                supplierData = supplierData.OrderBy(t => t.Name).ToList();
                supplierData = supplierData.Skip(currentPageIndex * request.rows).Take(request.rows).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = supplierData
            };

            return Json(jsonData);
        }

        public JsonResult Update(Supplier s)
        {
            SupplierViewModel supp = new SupplierViewModel();
            supp.UpdateSupplier(s);

            return Json(true);
        }

        public JsonResult Delete(string id)
        {
            SupplierViewModel supp = new SupplierViewModel();
            supp.DeleteSupplier(id);

            return Json(true);
        }
    }
}
