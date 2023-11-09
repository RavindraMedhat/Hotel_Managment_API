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
    public class RoomCategoryTBsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;


        public RoomCategoryTBsController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
  

        // GET: api/RoomCategoryTBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomCategoryTB>>> GetRoomCategoryTB()
        {
            return await _context.roomCategoryTB.ToListAsync();
        }

        
        // GET: api/RoomCategoryTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomCategoryTB>> GetRoomCategoryTB(int id)
        {
            var roomCategoryTB = await _context.roomCategoryTB.FindAsync(id);

            if (roomCategoryTB == null)
            {
                return NotFound();
            }

            return roomCategoryTB;
        }

        // PUT: api/RoomCategoryTBs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomCategoryTB(int id, RoomCategoryTB roomCategoryTB)
        {
            if (id != roomCategoryTB.Category_ID)
            {
                return BadRequest();
            }

            _context.Entry(roomCategoryTB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomCategoryTBExists(id))
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

        // POST: api/RoomCategoryTBs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RoomCategoryTB>> PostRoomCategoryTB(RoomCategoryTB roomCategoryTB)
        {
            _context.roomCategoryTB.Add(roomCategoryTB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomCategoryTB", new { id = roomCategoryTB.Category_ID }, roomCategoryTB);
        }

        // DELETE: api/RoomCategoryTBs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomCategoryTB>> DeleteRoomCategoryTB(int id)
        {
            var roomCategoryTB = await _context.roomCategoryTB.FindAsync(id);
            if (roomCategoryTB == null)
            {
                return NotFound();
            }

            _context.roomCategoryTB.Remove(roomCategoryTB);
            await _context.SaveChangesAsync();

            return roomCategoryTB;
        }

        private bool RoomCategoryTBExists(int id)
        {
            return _context.roomCategoryTB.Any(e => e.Category_ID == id);
        }
    }
}
