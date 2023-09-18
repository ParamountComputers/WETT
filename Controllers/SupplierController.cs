using Microsoft.AspNetCore.Mvc;
using WETT.Data;
using WETT.Models;
using System.Linq;
using System.Linq.Dynamic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WETT.Controllers
{
    public class SupplierController : Controller
    {
        private Supplier db = new Supplier();
        public static string currentLob;

        //ty add
        private readonly WETT_DBContext _context;
        public SupplierController(WETT_DBContext context)
        {
            _context = context;
        }


        //GET: Products
        public async Task<IActionResult> Index()
        {
            currentLob = "LIQ";
            var wETT_DBContext = _context.Suppliers.Where(a => a.ActiveFlag == "Y");
            return View(await wETT_DBContext.ToListAsync());
        }

		//[Route("Report/{SupplierId:int}")]
		public IActionResult SupplierReport(int SupplierId)
		{
			return View();
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

            var supplierData = (_context.Suppliers.Where(a => a.ActiveFlag == "Y" && a.LobCode.Trim() == "LIQ")).ToList();


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

            Supplier r = _context.Suppliers.Single(e => e.SupplierId == s.SupplierId);
            r.SupplierCode = s.SupplierCode;
            r.Name = s.Name;
            r.Address1 = s.Address1;
            r.Address2 = s.Address2;
            r.City = s.City;
            r.Province = s.Province;
            r.PostalCode = s.PostalCode;
            r.GeneralPhone = s.GeneralPhone;
            r.Contact1Name = s.Contact1Name;
            r.UpdateTimestamp = DateTime.Now;
            r.UpdateUserId = User.Identity.Name;

            _context.SaveChanges();


            return Json(true);
        }

        public JsonResult Delete(int id)
        {
            Supplier r = _context.Suppliers.Single(e => e.SupplierId == id);
            r.ActiveFlag = "N";
            _context.SaveChanges();

            /* Supplier r = _context.Suppliers.Where(a => a.SupplierId == s.SupplierId).First();
             if(r !=null)
             _context.Suppliers.Remove(s);
             _context.SaveChanges();

             return Json(true);
            */
            //Supplier r = _context.Suppliers.Include(a => a.SupplierId).Single(a2 => a2.SupplierId == s.SupplierId);
            /*  _context.Entry(s).State = EntityState.Modified;
              _context.Suppliers.Remove(s);
              _context.SaveChanges();

            */
            return Json(true);
        }

        public JsonResult Add(Supplier s)
        {

            s.ActiveFlag = "Y";
            s.LobCode = currentLob;
            s.InsertTimestamp = DateTime.Now;
            s.InsertUserId = User.Identity.Name;
            s.UpdateTimestamp = DateTime.Now;
            s.UpdateUserId = User.Identity.Name;
            _context.Suppliers.Add(s);
            _context.SaveChanges();

            return Json(true);
        }
        public IActionResult SelectLob(string data)
        {
            if (data == "0") {
                currentLob = "LIQ";
            }
            else {
                currentLob = "CAN";
            }

            return Json(true);
        }
    }
}
