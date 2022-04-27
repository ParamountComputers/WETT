using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WETT.Data;

namespace WETT.Controllers
{
    public class CustomersController : Controller
    {
        private readonly WETT_DBContext _context;

        public CustomersController(WETT_DBContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var wETT_DBContext = _context.Customers.Include(c => c.CallFrequency).Include(c => c.Cdos).Include(c => c.CustomerSource).Include(c => c.CustomerStatusCodeNavigation).Include(c => c.CustomerTypeCodeNavigation).Include(c => c.Segment).Include(c => c.Territory);
            return View(await wETT_DBContext.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.CallFrequency)
                .Include(c => c.Cdos)
                .Include(c => c.CustomerSource)
                .Include(c => c.CustomerStatusCodeNavigation)
                .Include(c => c.CustomerTypeCodeNavigation)
                .Include(c => c.Segment)
                .Include(c => c.Territory)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["CallFrequencyId"] = new SelectList(_context.CallFrequencies, "CallFrequencyId", "Description");
            ViewData["CdosId"] = new SelectList(_context.Cdos, "CdosId", "Description");
            ViewData["CustomerSourceId"] = new SelectList(_context.CustomerSources, "CustomerSourceId", "Description");
            ViewData["CustomerStatusCode"] = new SelectList(_context.CustomerStatuses, "CustomerStatusCode", "CustomerStatusCode");
            ViewData["CustomerTypeCode"] = new SelectList(_context.CustomerTypes, "CustomerTypeCode", "CustomerTypeCode");
            ViewData["SegmentId"] = new SelectList(_context.Segments, "SegmentId", "Description");
            ViewData["TerritoryId"] = new SelectList(_context.Territories, "TerritoryId", "Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerTypeCode,CustomerSourceId,CallFrequencyId,TerritoryId,SegmentId,CdosId,MbllCustomerNo,LicenceNumber,CustomerStatusCode,Name,Address,City,Province,PostalCode,Country,ContactName,Phone1Type,Phone1,Phone2Type,Phone2,Phone3Type,Phone3,ContactEmail,DeletedFlag,DeletedDate,InsertUserId,InsertTimestamp,UpdateUserId,UpdateTimestamp")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CallFrequencyId"] = new SelectList(_context.CallFrequencies, "CallFrequencyId", "Description", customer.CallFrequencyId);
            ViewData["CdosId"] = new SelectList(_context.Cdos, "CdosId", "Description", customer.CdosId);
            ViewData["CustomerSourceId"] = new SelectList(_context.CustomerSources, "CustomerSourceId", "Description", customer.CustomerSourceId);
            ViewData["CustomerStatusCode"] = new SelectList(_context.CustomerStatuses, "CustomerStatusCode", "CustomerStatusCode", customer.CustomerStatusCode);
            ViewData["CustomerTypeCode"] = new SelectList(_context.CustomerTypes, "CustomerTypeCode", "CustomerTypeCode", customer.CustomerTypeCode);
            ViewData["SegmentId"] = new SelectList(_context.Segments, "SegmentId", "Description", customer.SegmentId);
            ViewData["TerritoryId"] = new SelectList(_context.Territories, "TerritoryId", "Name", customer.TerritoryId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CallFrequencyId"] = new SelectList(_context.CallFrequencies, "CallFrequencyId", "Description", customer.CallFrequencyId);
            ViewData["CdosId"] = new SelectList(_context.Cdos, "CdosId", "Description", customer.CdosId);
            ViewData["CustomerSourceId"] = new SelectList(_context.CustomerSources, "CustomerSourceId", "Description", customer.CustomerSourceId);
            ViewData["CustomerStatusCode"] = new SelectList(_context.CustomerStatuses, "CustomerStatusCode", "CustomerStatusCode", customer.CustomerStatusCode);
            ViewData["CustomerTypeCode"] = new SelectList(_context.CustomerTypes, "CustomerTypeCode", "CustomerTypeCode", customer.CustomerTypeCode);
            ViewData["SegmentId"] = new SelectList(_context.Segments, "SegmentId", "Description", customer.SegmentId);
            ViewData["TerritoryId"] = new SelectList(_context.Territories, "TerritoryId", "Name", customer.TerritoryId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CustomerId,CustomerTypeCode,CustomerSourceId,CallFrequencyId,TerritoryId,SegmentId,CdosId,MbllCustomerNo,LicenceNumber,CustomerStatusCode,Name,Address,City,Province,PostalCode,Country,ContactName,Phone1Type,Phone1,Phone2Type,Phone2,Phone3Type,Phone3,ContactEmail,DeletedFlag,DeletedDate,InsertUserId,InsertTimestamp,UpdateUserId,UpdateTimestamp")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["CallFrequencyId"] = new SelectList(_context.CallFrequencies, "CallFrequencyId", "Description", customer.CallFrequencyId);
            ViewData["CdosId"] = new SelectList(_context.Cdos, "CdosId", "Description", customer.CdosId);
            ViewData["CustomerSourceId"] = new SelectList(_context.CustomerSources, "CustomerSourceId", "Description", customer.CustomerSourceId);
            ViewData["CustomerStatusCode"] = new SelectList(_context.CustomerStatuses, "CustomerStatusCode", "CustomerStatusCode", customer.CustomerStatusCode);
            ViewData["CustomerTypeCode"] = new SelectList(_context.CustomerTypes, "CustomerTypeCode", "CustomerTypeCode", customer.CustomerTypeCode);
            ViewData["SegmentId"] = new SelectList(_context.Segments, "SegmentId", "Description", customer.SegmentId);
            ViewData["TerritoryId"] = new SelectList(_context.Territories, "TerritoryId", "Name", customer.TerritoryId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.CallFrequency)
                .Include(c => c.Cdos)
                .Include(c => c.CustomerSource)
                .Include(c => c.CustomerStatusCodeNavigation)
                .Include(c => c.CustomerTypeCodeNavigation)
                .Include(c => c.Segment)
                .Include(c => c.Territory)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
