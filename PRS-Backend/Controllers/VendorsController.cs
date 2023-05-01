using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRS_Backend.Models;

namespace PRS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public VendorsController(PrsDbContext context)
        {
            _context = context;
        }

        [HttpGet("po/{vendorId}")]
        public async Task<ActionResult<Po>> CreatePo(int vendorId)
        {
            Po po = new Po();
            po.Vendor = await _context.Vendors.FindAsync(vendorId);
            var polines = (from vend in _context.Vendors
                          join prod in _context.Products
                          on vend.Id equals prod.VendorId
                          join reqL in _context.RequestLines
                          on prod.Id equals reqL.ProductId
                          join req in _context.Requests
                          on reqL.RequestId equals req.Id
                          where req.Status == "APPROVED"
                          select new
                          {
                              prod.Id,
                              Product = prod.Name,
                              reqL.Quantity,
                              prod.Price,
                              LineTotal = prod.Price * reqL.Quantity
                          });
            SortedList<int, Poline> sortedLines = new SortedList<int, Poline>();
            decimal poTotal = 0;
            foreach (var poline in polines)
            {
                if(!sortedLines.ContainsKey(poline.Id))
                {
                    Poline polineAdd = new Poline()
                    {
                        Product = poline.Product,
                        Quantity = 0,
                        Price = poline.Price,
                        LineTotal = poline.LineTotal
                    };
                    sortedLines.Add(poline.Id, polineAdd);
                    poTotal += polineAdd.LineTotal;
                }
                sortedLines[poline.Id].Quantity += poline.Quantity;
            }

            var addToPolinesProp = sortedLines.Values;
            po.Polines = addToPolinesProp;
            po.PoTotal = poTotal;
            return po;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
            return await _context.Vendors.ToListAsync();
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // PUT: api/Vendors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vendors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}
