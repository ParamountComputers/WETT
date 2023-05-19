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
    public class CustomersController : Controller
    {
        private Customer db = new Customer();

        //ty add
        private readonly WETT_DBContext _context;
        public CustomersController(WETT_DBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var wETT_DBContext = _context.Customers.Where(w=>w.ActiveFlag== true);
            return View(await wETT_DBContext.ToListAsync());
        }

        public JsonResult GetAll(JqGridViewModel request)
        {
            var wETT_DBContext = _context.Customers;

            var customerData = (_context.Customers.Where(w => w.ActiveFlag == true)).ToList(); 


            bool issearch = request._search && request.searchfilters.rules.Any(a => !string.IsNullOrEmpty(a.data));

            if (issearch)
                foreach (Rule rule in request.searchfilters.rules.Where(a => !string.IsNullOrEmpty(a.data)))
                {
                    switch (rule.field)
                    {
                        //case "LOB":
                        //    customerData = customerData.Where(w => w.Name.Contains("")).ToList();
                        //    break;
                        case "name":
                            customerData = customerData.Where(w => w.Name.Contains(rule.data)).ToList();
                            break;
                       
                    }
                }

            int totalRecords = customerData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)request.rows);
            int currentPageIndex = request.page - 1;

            //Kept default sorting to Supplier Name, implement sorting for other fields using switchcase
            if (request.sord.ToUpper() == "DESC")
            {
                customerData = customerData.OrderByDescending(t => t.Name).ToList();
                customerData = customerData.Skip(currentPageIndex * request.rows).Take(request.rows).ToList();
            }
            else
            {
                customerData = customerData.OrderBy(t => t.Name).ToList();
                customerData = customerData.Skip(currentPageIndex * request.rows).Take(request.rows).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                request.page,
                records = totalRecords,
                rows = customerData
            };

            return Json(jsonData);
        }

        public JsonResult Update(Customer s)
        {
            Customer r = _context.Customers.Single(e => e.CustomerId == s.CustomerId);
            r.CustomerTypeCode = s.CustomerTypeCode;
            r.CustomerSourceId = s.CustomerSourceId;
            r.CallFrequencyId = s.CallFrequencyId;
            r.TerritoryId = s.TerritoryId;
            r.SegmentId = s.SegmentId;
            r.CdosId = s.CdosId;
            r.MbllCustomerNo = s.MbllCustomerNo;
            r.LicenceNumber = s.LicenceNumber;
            r.CustomerStatusCode = s.CustomerStatusCode;
            r.Name = s.Name;
            r.Address =  s.Address;
            r.City = s.City;
            r.Province = s.Province;
            r.PostalCode = s.PostalCode;
            r.Country = s.Country;
            r.ContactName = s.ContactName;
            r.Phone1Type = s.Phone1Type;
            r.Phone1 = s.Phone1;
            r.Phone2Type = s.Phone2Type;
            r.Phone2 = s.Phone2;
            r.Phone3Type = s.Phone3Type;
            r.Phone3 = s.Phone3;
            r.ContactEmail = s.ContactEmail;
            r.ActiveFlag = true;
            r.UpdateTimestamp = DateTime.Now;
            r.UpdateUserId = User.Identity.Name;
            _context.SaveChanges();


            return Json(true);
        }

        public JsonResult Delete(long id)
        {
            Customer r = _context.Customers.Single(e => e.CustomerId == id);
            //r.DeletedDate= DateTime.Today;
            r.ActiveFlag = false;
            _context.SaveChanges();
            return Json(true);
        }

        public JsonResult Add(Customer s)
        {
            s.ActiveFlag = true;
            s.InsertUserId = User.Identity.Name;
            s.InsertTimestamp = DateTime.Now;
            s.UpdateUserId = User.Identity.Name;
            s.UpdateTimestamp = DateTime.Now;
            _context.Customers.Add(s);
            _context.SaveChanges();

            return Json(true); 
        }
        public IActionResult CreateCallFrequencyList()
        {
            var invAdjData = from a in _context.CallFrequencies
                             select new
                             {
                                 value = a.CallFrequencyId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
        public IActionResult CreateCdosList()
        {
            var invAdjData = from a in _context.Cdos
                             select new
                             {
                                 value = a.CdosId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
    public IActionResult CreateCustomerSourceList()
        {
            var invAdjData = from a in _context.CustomerSources
                             select new
                             {
                                 value = a.CustomerSourceId,
                                 text = a.Description
                             };
            return Json(invAdjData);
        }
    public IActionResult CreateSegmentList()
        {
            var invAdjData = from a in _context.Segments
                             select new
                             {
                                 value = a.SegmentId,
                                 text = a.Description
                             };
            return Json(invAdjData);
         }
    public IActionResult CreateTerritoryList()
        {
            var invAdjData = from a in _context.Territories
                             select new
                             {
                                 value = a.TerritoryId,
                                 text = a.Name
                             };
            return Json(invAdjData);
        }

    }
}