using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Management.Models;
using Hotel_Managment_API.DBContext;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelTBsController : ControllerBase
    {
        private readonly DataContext _context;

        public HotelTBsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/HotelTBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelTB>>> Gethotels()
        {
            return await _context.hotels.ToListAsync();
        }

        // GET: api/HotelTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelTB>> GetHotelTB(int id)
        {
            var hotelTB = await _context.hotels.FindAsync(id);

            if (hotelTB == null)
            {
                return NotFound();
            }

            return hotelTB;
        }

        // PUT: api/HotelTBs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotelTB(int id, HotelTB hotelTB)
        {
            if (id != hotelTB.Hotel_ID)
            {
                return BadRequest();
            }

            _context.Entry(hotelTB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelTBExists(id))
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

        // POST: api/HotelTBs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HotelTB>> PostHotelTB(HotelTB hotelTB)
        {
            _context.hotels.Add(hotelTB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotelTB", new { id = hotelTB.Hotel_ID }, hotelTB);
        }

        // DELETE: api/HotelTBs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelTB>> DeleteHotelTB(int id)
        {
            var hotelTB = await _context.hotels.FindAsync(id);
            if (hotelTB == null)
            {
                return NotFound();
            }

            _context.hotels.Remove(hotelTB);
            await _context.SaveChangesAsync();

            return hotelTB;
        }

        private bool HotelTBExists(int id)
        {
            return _context.hotels.Any(e => e.Hotel_ID == id);
        }
    }
}
