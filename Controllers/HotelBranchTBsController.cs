using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Managment_API.DBContext;
using Hotel_Managment_API.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Hotel_Managment_API.ViewModels;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelBranchTBsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;


        public HotelBranchTBsController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/HotelBranchTBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelBranchViewModelForIndex>>> GethotelBranchTBs()
        {
            List<ImageMasterTB> HotelsImageUrls = (from i in await _context.imageMasterTBs.ToListAsync()
                                                   where i.ReferenceTB_Name == "HotelBranch"
                                                   select i).ToList();
            List<(int, string)> Hoteldata = ( from h in await _context.hotels.ToListAsync()
                                            select (h.Hotel_ID,h.Hotel_Name)).ToList() ;

            //return await _context.hotelBranchTBs.ToListAsync();

            List<HotelBranchViewModelForIndex> data = (from hb in await _context.hotelBranchTBs.ToListAsync()
                                                       where hb.Delete_Flag == false
                                                       select new HotelBranchViewModelForIndex
                                                       {
                                                           Branch_ID = hb.Branch_ID,
                                                           Branch_Name=hb.Branch_Name,
                                                           Hotel_Name = (from h in Hoteldata
                                                                         where h.Item1 == hb.Hotel_ID
                                                                         select h.Item2).FirstOrDefault(),
                                                           Image_URl = HotelsImageUrls.Where(i => i.Reference_ID == hb.Branch_ID).Select(i => "http://localhost:17312/api/img/hotelBranch/" + i.Image_URl).ToList()
                                                       }).ToList();
            return data;
        }

        //get hotel branch for dropdown

        [HttpGet("ForDropDown")]
        public async Task<ActionResult<IEnumerable<BranchNameAndIdViewModel>>> GetHotelBranchForDropDown()
        {
            var Branch = await _context.hotelBranchTBs
                .Where(b => !b.Delete_Flag)
                .Select(b => new BranchNameAndIdViewModel
                {
                    Branch_ID = b.Branch_ID,
                    Branch_Name = b.Branch_Name
                })
                .ToListAsync();

            return Branch;
        }
        // GET: api/HotelBranchTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelBranchTB>> GetHotelBranchTB(int id)
        {
            var hotelBranchTB = await _context.hotelBranchTBs.FindAsync(id);

            if (hotelBranchTB == null)
            {
                return NotFound();
            }

            return hotelBranchTB;
        }

        // PUT: api/HotelBranchTBs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotelBranchTB(int id, HotelBranchTB hotelBranchTB)
        {
            if (id != hotelBranchTB.Branch_ID)
            {
                return BadRequest();
            }

            _context.Entry(hotelBranchTB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelBranchTBExists(id))
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

        // POST: api/HotelBranchTBs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HotelBranchTB>> PostHotelBranchTB([FromForm] HotelBranchCreateModel hotelBranchcreatemodel)
        {
            HotelBranchTB hotelBranchTB = new HotelBranchTB { Active_Flag = hotelBranchcreatemodel.Active_Flag, Branch_Address = hotelBranchcreatemodel.Branch_Address, Branch_Contact_No = hotelBranchcreatemodel.Branch_Contact_No, Branch_Contect_Person = hotelBranchcreatemodel.Branch_Contect_Person, Delete_Flag = hotelBranchcreatemodel.Delete_Flag, Branch_Email_Adderss = hotelBranchcreatemodel.Branch_Email_Adderss, Branch_Description = hotelBranchcreatemodel.Branch_Description, Branch_map_coordinate = hotelBranchcreatemodel.Branch_map_coordinate, Branch_Name = hotelBranchcreatemodel.Branch_Name, sortedfield = hotelBranchcreatemodel.sortedfield,Hotel_ID=hotelBranchcreatemodel.Hotel_ID};

            _context.hotelBranchTBs.Add(hotelBranchTB);
            await _context.SaveChangesAsync();

            if (hotelBranchcreatemodel.Photos != null && hotelBranchcreatemodel.Photos.Count > 0)
            {
                foreach (IFormFile photo in hotelBranchcreatemodel.Photos)
                {
                    String upfolder = Path.Combine(_env.ContentRootPath, "./Public/images/hotelBranch");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    String filePath = Path.Combine(upfolder, uniqueFileName);
                    using (var filesStrime = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(filesStrime);
                    }

                    ImageMasterTB im = new ImageMasterTB
                    {
                        Image_URl = uniqueFileName,
                        Reference_ID = hotelBranchTB.Branch_ID,
                        ReferenceTB_Name = "HotelBranch",
                        Active_Flag = true,
                        Delete_Flag = false,
                        sortedfield = 99
                    };
                    _context.imageMasterTBs.Add(im);
                    await _context.SaveChangesAsync();

                }

            }
            _context.hotelBranchTBs.Add(hotelBranchTB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotelBranchTB", new { id = hotelBranchTB.Branch_ID }, hotelBranchTB);
        }

        // DELETE: api/HotelBranchTBs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelBranchTB>> DeleteHotelBranchTB(int id)
        {
            var hotelBranchTB = await _context.hotelBranchTBs.FindAsync(id);
            if (hotelBranchTB == null)
            {
                return NotFound();
            }

            _context.hotelBranchTBs.Remove(hotelBranchTB);
            await _context.SaveChangesAsync();

            return hotelBranchTB;
        }

        private bool HotelBranchTBExists(int id)
        {
            return _context.hotelBranchTBs.Any(e => e.Branch_ID == id);
        }
    }
}
public class BranchNameAndIdViewModel
{
    public int Branch_ID { get; set; }
    public string Branch_Name { get; set; }
}