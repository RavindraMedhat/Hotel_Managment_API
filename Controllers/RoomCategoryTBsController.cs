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
            List<HotelBranchTB> hotelBranchTBs = await _context.hotelBranchTBs.ToListAsync();

            List<RoomCategoryTBForDetail> roomCategoryTBForDetail = (from rc in await _context.roomCategoryTB.ToListAsync()
                                                                    where rc.Delete_Flag== false 
                                                                    select new RoomCategoryTBForDetail
                                                                    {   
                                                                        Category_ID = rc.Category_ID,
                                                                        Category_Name = rc.Category_Name,
                                                                        Branch_Name =   (from hb in hotelBranchTBs
                                                                                        where hb.Branch_ID == hb.Branch_ID
                                                                                        select hb.Branch_Name).FirstOrDefault(),
                                                                        Active_Flag=rc.Active_Flag,
                                                                        Delete_Flag=rc.Delete_Flag,
                                                                        Description=rc.Description,
                                                                        sortedfield=rc.sortedfield
                                                                    }).ToList();



            return roomCategoryTBForDetail;
        }

        
        // GET: api/RoomCategoryTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomCategoryTBForDetail>> GetRoomCategoryTB(int id)
        {
            var roomCategoryTB = await _context.roomCategoryTB.FindAsync(id);

            if (roomCategoryTB == null)
            {
                return NotFound();
            }

            var hotelBranch = await _context.hotelBranchTBs.FindAsync(roomCategoryTB.Branch_ID);

            RoomCategoryTBForDetail roomCategoryTBForDetail = new RoomCategoryTBForDetail { Active_Flag = roomCategoryTB.Active_Flag, Branch_Name = hotelBranch.Branch_Name, Category_ID = roomCategoryTB.Category_ID,Category_Name= roomCategoryTB.Category_Name,Delete_Flag= roomCategoryTB.Delete_Flag,Description= roomCategoryTB.Description,sortedfield= roomCategoryTB.sortedfield };

            return roomCategoryTBForDetail;
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
