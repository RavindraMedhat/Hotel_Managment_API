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
using Hotel_Managment_API.ViewModels;

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
        public async Task<ActionResult<IEnumerable<RoomCategoryTBForDetail>>> GetRoomCategoryTB()
        {
            List<(int, string)> HotelBranchdata = (from hb in await _context.hotelBranchTBs.ToListAsync()
                                             select (hb.Branch_ID, hb.Branch_Name)).ToList();
            List<RoomCategoryTBForDetail> roomCategoryTBForDetail = (from rcd in await _context.roomCategoryTB.ToListAsync()
                                                                     where rcd.Delete_Flag == false
                                                                     select new RoomCategoryTBForDetail
                                                                     {
                                                                         Category_ID = rcd.Category_ID,
                                                                         Category_Name = rcd.Category_Name,
                                                                         Description = rcd.Description,
                                                                         Active_Flag = rcd.Active_Flag,
                                                                         Delete_Flag = rcd.Delete_Flag,
                                                                         sortedfield = rcd.sortedfield,
                                                                         Branch_Name = (from hb in HotelBranchdata
                                                                                        where hb.Item1 == rcd.Branch_ID
                                                                                        select hb.Item2).FirstOrDefault()
                                                                     }).ToList();
            return roomCategoryTBForDetail;
        }
        [HttpGet("ByBranchID/{id}")]
        public async Task<ActionResult<IEnumerable<RoomCategoryTBforcheckbox>>> GetHotelBranchByHotelID(int id)
        {
            List<RoomCategoryTBforcheckbox> roomCategoryTB = (from rc in await _context.roomCategoryTB.ToListAsync()
                                                             where rc.Delete_Flag == false && rc.Branch_ID == id
                                                   select new RoomCategoryTBforcheckbox { Category_ID= rc.Category_ID, Category_Name=rc.Category_Name }).ToList();
            
            return roomCategoryTB;
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

public class RoomCategoryTBforcheckbox
{
    public int Category_ID { get; set; }
    public string Category_Name { get; set; }
}