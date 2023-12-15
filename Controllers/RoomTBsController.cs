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
using System.IO;
using Hotel_Managment_API.ViewModels;

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
        public async Task<ActionResult<IEnumerable<RoomTBForIndex>>> GetHotelBranchByHotelID(int id)
        {
            //List<RoomTB> roomTB = (from r in await _context.RoomTB.ToListAsync()
            //                       where r.Delete_Flag == false && r.Branch_ID == id
            //                       select r).ToList();

            //return roomTB;
            List<ImageMasterTB> HotelsImageUrls = (from i in await _context.imageMasterTBs.ToListAsync()
                                                   where i.ReferenceTB_Name == "RoomTB"
                                                   select i).ToList();

            List<RoomTBForIndex> data = (from r in await _context.RoomTB.ToListAsync()
                                         where r.Delete_Flag == false && r.Branch_ID == id
                                         select new RoomTBForIndex
                                         {
                                             Room_ID = r.Room_ID,
                                             Room_No = r.Room_No,
                                             Iminity_NoOfBed = r.Iminity_NoOfBed,
                                             Room_Price = r.Room_Price,
                                             Category_ID = r.Category_ID,
                                             Image_URl = HotelsImageUrls.Where(i => i.Reference_ID == r.Room_ID)
                                             .Select(i => "http://localhost:17312/api/img/HotelRoom/" + i.Image_URl).ToList()
                                         }).ToList();
            return data;
        }

        // GET: api/RoomTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomViewModelForDetails>> GetRoomTB(int id)
        {
            RoomTB roomTB = await _context.RoomTB.FindAsync(id);
           
            List<string> ImageUrls = (from i in await _context.imageMasterTBs.ToListAsync()
                                                        where i.ReferenceTB_Name == "RoomTB" && i.Reference_ID==roomTB.Room_ID
                                                        select @"http://localhost:17312/api/img/HotelRoom/" + i.Image_URl).ToList();
            string C_Name = (from c in await _context.roomCategoryTB.ToListAsync()
                             where  c.Category_ID == roomTB.Category_ID
                             select c.Category_Name).FirstOrDefault();
            RoomViewModelForDetails roomViewModelForDetails = new RoomViewModelForDetails()
            {
                Room_ID=roomTB.Room_ID,
                Category_Name = C_Name,
                Image_URl= ImageUrls,
                Iminity_Bath=roomTB.Iminity_Bath,
                Iminity_NoOfBed=roomTB.Iminity_NoOfBed,
                Iminity_Pool=roomTB.Iminity_Pool,
                Room_Description=roomTB.Room_Description,
                Room_No=roomTB.Room_No,
                Room_Price=roomTB.Room_Price,
                Branch_ID=roomTB.Branch_ID,
            };

            if (roomViewModelForDetails == null)
            {
                return NotFound();
            }

            return roomViewModelForDetails;
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
        public async Task<ActionResult<RoomTB>> PostRoomTB([FromForm] RoomCreateModel roomCreateModel)
        {
            RoomTB roomTB = new RoomTB { Active_Flag = roomCreateModel.Active_Flag, Branch_ID = roomCreateModel.Branch_ID, Category_ID = roomCreateModel.Category_ID,Delete_Flag=roomCreateModel.Delete_Flag,Hotel_ID=roomCreateModel.Hotel_ID,Iminity_Bath=roomCreateModel.Iminity_Bath,Iminity_NoOfBed=roomCreateModel.Iminity_NoOfBed,Iminity_Pool=roomCreateModel.Iminity_Pool,Room_Description=roomCreateModel.Room_Description,Room_No=roomCreateModel.Room_No,Room_Price=roomCreateModel.Room_Price,sortedfield=roomCreateModel.sortedfield};
            _context.RoomTB.Add(roomTB);
            await _context.SaveChangesAsync();

            if (roomCreateModel.Photos != null && roomCreateModel.Photos.Count > 0)
            {
                foreach (IFormFile photo in roomCreateModel.Photos)
                {
                    String upfolder = Path.Combine(_env.ContentRootPath, "./Public/images/HotelRoom");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    String filePath = Path.Combine(upfolder, uniqueFileName);
                    using (var filesStrime = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(filesStrime);
                    }

                    ImageMasterTB im = new ImageMasterTB
                    {
                        Image_URl = uniqueFileName,
                        Reference_ID = roomTB.Room_ID,
                        ReferenceTB_Name = "RoomTB",
                        Active_Flag = true,
                        Delete_Flag = false,
                        sortedfield = 99
                    };
                    _context.imageMasterTBs.Add(im);
                    await _context.SaveChangesAsync();

                }

            }
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
