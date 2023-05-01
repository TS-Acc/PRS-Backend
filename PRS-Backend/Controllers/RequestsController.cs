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
    public class RequestsController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public RequestsController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: /api/requests/reviews/{userId}
        // Gets requests in "REVIEW" status and not owned by the user with the primary key of id
        [HttpGet("/api/requests/reviews/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> RequestsInReviewStatus(int userId)
        {
            List<Request> requests = await _context.Requests.Include(x => x.Users).ToListAsync();

            List<Request> reviewRequests = new List<Request>();

            foreach (Request request in requests)
            {
                if (request.UserId != userId && request.Status == "REVIEW")
                {
                    reviewRequests.Add(request);
                }
                else
                {
                    continue;
                }
            }

            return reviewRequests;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.Include(x => x.Users).Include(x => x.RequestLines).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.Include(x => x.Users)
                                                 .Include(x => x.RequestLines).ThenInclude(x => x.Products)
                                                 .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: /api/requests/review/{id}
        // Sets the request to "REVIEW" status. If the total of the request is under $50 it's automatically set to "APPROVED"
        [HttpPut("/api/requests/review/{id}")]
        public async Task<IActionResult> ReviewRequest(int id)
        {
            Request? request = await _context.Requests.FindAsync(id);

            if (request is null)
            {
                return NotFound();
            }

            if (request.Total <= 50)
            {
                request.Status = "APPROVED";
                return await PutRequest(id, request);
            }

            request.Status = "REVIEW";
            request.RejectionReason = "";
            return await PutRequest(id, request);
        }

        // PUT: /api/requests/approve/{id}
        // Sets the status of the request for the id provided to "APPROVED"
        [HttpPut("/api/requests/approve/{id}")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            Request? request = await _context.Requests.FindAsync(id);

            if (request is null)
            {
                return NotFound();
            }
            request.Status = "APPROVED";
            return await PutRequest(id, request);
        }

        // PUT: /api/requests/reject/{id}
        // Sets the status of the request for the id provided to "REJECTED"
        [HttpPut("/api/requests/reject/{id}")]
        public async Task<IActionResult> RejectRequest(Request request)
        {
            if (request is null)
            {
                return NotFound();
            }
            request.Status = "REJECTED";
            return await PutRequest(request.Id, request);
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
