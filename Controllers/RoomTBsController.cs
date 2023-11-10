using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Managment_API.DBContext;
using Hotel_Managment_API.Models;
using Microsoft.AspNetCore.Hosting;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTBsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;


        public RoomTBsController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/RoomTBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomTB>>> GetRoomTB()
        {
            return await _context.RoomTB.ToListAsync();
        }

        [HttpGet("ByBranchID/{id}")]
        public async Task<ActionResult<IEnumerable<RoomTB>>> GetHotelBranchByHotelID(int id)
        {
            List<RoomTB> roomTB = (from r in await _context.RoomTB.ToListAsync()
                                   where r.Delete_Flag == false && r.Branch_ID == id
                                   select r).ToList();

            return roomTB;
        }

        // GET: api/RoomTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTB>> GetRoomTB(int id)
        {
            var roomTB = await _context.RoomTB.FindAsync(id);

            if (roomTB == null)
            {
                return NotFound();
            }

            return roomTB;
        }

        // PUT: api/RoomTBs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomTB(int id, RoomTB roomTB)
        {
            if (id != roomTB.Room_ID)
            {
                return BadRequest();
            }

            _context.Entry(roomTB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomTBExists(id))
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

        // POST: api/RoomTBs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RoomTB>> PostRoomTB(RoomTB roomTB)
        {
            _context.RoomTB.Add(roomTB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomTB", new { id = roomTB.Room_ID }, roomTB);
        }

        // DELETE: api/RoomTBs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomTB>> DeleteRoomTB(int id)
        {
            var roomTB = await _context.RoomTB.FindAsync(id);
            if (roomTB == null)
            {
                return NotFound();
            }

            _context.RoomTB.Remove(roomTB);
            await _context.SaveChangesAsync();

            return roomTB;
        }

        private bool RoomTBExists(int id)
        {
            return _context.RoomTB.Any(e => e.Room_ID == id);
        }
    }
}
