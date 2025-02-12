using FakeFlightBookingAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SharedModels;

namespace FakeFlightBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedFlightController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public BookedFlightController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("GetBookedFlightsByBookingID")]
        public async Task<ActionResult<BookedFlight>> GetBookedFlightsByBookingID([FromQuery] int BookingId)
        {
            var bookedFlight = await _context.BookedFlights.FindAsync(BookingId);
            if (bookedFlight == null)
            {
                return NotFound();
            }
            return bookedFlight;
        }

        [HttpGet("GetBookedFlightByID")]
        public async Task<ActionResult<BookedFlight>> GetBookedFlightByID([FromQuery] int id)
        {
            Console.WriteLine($"Fetching booked flights for User ID: {id}");

            var bookedFlights = await _context.BookedFlights
                .Where(f => f.CustomerId == id)  // Ensure it filters by CustomerId, not PK
                .ToListAsync();

            if (!bookedFlights.Any())
            {
                Console.WriteLine($"No flights found for User ID: {id}");
                return NotFound("No booked flights found for this user.");
            }

            return Ok(bookedFlights);
        }

        [HttpPost("AddBookedFlight")]
        public async Task<ActionResult<BookedFlight>> PostBookedFlight([FromBody] BookedFlight bookedFlight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate the UserID (ensure the user exists)
            var user = await _context.CustomerUsers.FindAsync(bookedFlight.CustomerId);
            if (user == null)
            {
                return BadRequest("Invalid User ID.");
            }

            // Add the booked flight to the database
            bookedFlight.BookingDate = DateTime.UtcNow; // Ensure booking date is set
            _context.BookedFlights.Add(bookedFlight);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookedFlightByID), new { id = bookedFlight.BookingId }, bookedFlight);
        }

        [HttpPut("UpdateBookedFlightByID")]
        public async Task<IActionResult> PutBookedFlight([FromQuery] int id, BookedFlight bookedFlight)
        {
            if (id != bookedFlight.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(bookedFlight).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("DeleteBookedFlightByID")]
        public async Task<IActionResult> DeleteBookedFlight([FromQuery] int id)
        {
            var bookedFlight = await _context.BookedFlights.FindAsync(id);
            if (bookedFlight == null)
            {
                return NotFound();
            }

            _context.BookedFlights.Remove(bookedFlight);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
