using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Managment_API.Models;
using Hotel_Managment_API.DBContext;
using Hotel_Managment_API.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelTBsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public HotelTBsController(DataContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }

        // GET: api/HotelTBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelViewModelForIndex>>> Gethotels()
        {
            List<ImageMasterTB> HotelsImageUrls = (from i in await _context.imageMasterTBs.ToListAsync()
                                                   where i.ReferenceTB_Name=="Hotel"
                                                   select i).ToList();

            List<HotelViewModelForIndex> data = (from h in await _context.hotels.ToListAsync()
                                                 where h.Delete_Flag==false
                                                select new HotelViewModelForIndex
                                                {
                                                    Hotel_ID = h.Hotel_ID,
                                                    Hotel_Name = h.Hotel_Name,

                                                    Image_URl = HotelsImageUrls.Where(i=> i.Reference_ID==h.Hotel_ID).Select(i=> "http://localhost:17312/api/img/hotels/"+i.Image_URl).ToList()
                                                }).ToList();
            return data;
        }

        // GET: api/HotelTBs/ForDropDown
        [HttpGet("ForDropDown")]
        public async Task<ActionResult<IEnumerable<HotelNameAndIdViewModel>>> GetHotelsForDropDown()
        {
            var hotels = await _context.hotels
                .Where(h => !h.Delete_Flag)
                .Select(h => new HotelNameAndIdViewModel
                {
                    Hotel_ID = h.Hotel_ID,
                    Hotel_Name = h.Hotel_Name
                })
                .ToListAsync();

            return hotels;
        }

        // GET: api/HotelTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelViewModelForDetails>> GetHotelTB(int id)
        {

            var hotelTB = await _context.hotels.FindAsync(id);
            List<string> AllImageUrls = (from i in await _context.imageMasterTBs.ToListAsync()
                                                where i.Reference_ID==hotelTB.Hotel_ID && i.ReferenceTB_Name=="Hotel"
                                                   select "http://localhost:17312/api/img/hotels/" + i.Image_URl).ToList();

           
            HotelViewModelForDetails hotelViewModelForDetails = new HotelViewModelForDetails { Active_Flag= hotelTB.Active_Flag,Address= hotelTB.Address,Contact_No= hotelTB.Contact_No,Contect_Person= hotelTB.Contect_Person,Delete_Flag= hotelTB.Delete_Flag,Email_Adderss= hotelTB.Email_Adderss,Hotel_Description= hotelTB.Hotel_Description,Hotel_map_coordinate= hotelTB.Hotel_map_coordinate,Hotel_Name= hotelTB.Hotel_Name,sortedfield= hotelTB.sortedfield,Standard_check_In_Time= hotelTB.Standard_check_In_Time,Standard_check_out_Time= hotelTB.Standard_check_out_Time,Hotel_ID=hotelTB.Hotel_ID , images= AllImageUrls };

        

            if (hotelTB == null)
            {
                return NotFound();
            }

            return hotelViewModelForDetails;
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
        public async Task<ActionResult<HotelTB>> PostHotelTB([FromForm] HotelTBCreatModel hotelTBCreat)
        {
            HotelTB hotelTB = new HotelTB {Active_Flag=hotelTBCreat.Active_Flag,Address=hotelTBCreat.Address,Contact_No= hotelTBCreat.Contact_No,Contect_Person= hotelTBCreat.Contect_Person,Delete_Flag= hotelTBCreat.Delete_Flag,Email_Adderss= hotelTBCreat.Email_Adderss,Hotel_Description= hotelTBCreat.Hotel_Description,Hotel_map_coordinate= hotelTBCreat.Hotel_map_coordinate,Hotel_Name= hotelTBCreat.Hotel_Name,sortedfield= hotelTBCreat.sortedfield,Standard_check_In_Time= hotelTBCreat.Standard_check_In_Time,Standard_check_out_Time= hotelTBCreat.Standard_check_out_Time };

            _context.hotels.Add(hotelTB);
            await _context.SaveChangesAsync();

            if (hotelTBCreat.Photos != null && hotelTBCreat.Photos.Count > 0)
            {
                foreach (IFormFile photo in hotelTBCreat.Photos)
                {
                    String upfolder = Path.Combine(_env.ContentRootPath, "./Public/images/hotels");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    String filePath = Path.Combine(upfolder, uniqueFileName);
                    using (var filesStrime = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(filesStrime);
                    }

                    ImageMasterTB im = new ImageMasterTB
                    {
                        Image_URl = uniqueFileName,
                        Reference_ID = hotelTB.Hotel_ID,
                        ReferenceTB_Name = "Hotel",
                        Active_Flag = true,
                        Delete_Flag = false,
                        sortedfield = 99
                    };
                    _context.imageMasterTBs.Add(im);
                    await _context.SaveChangesAsync();

                }

            }


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
            hotelTB.Delete_Flag = true;
           // _context.hotels.Remove(hotelTB);
            await _context.SaveChangesAsync();  

            return hotelTB;
        }

        private bool HotelTBExists(int id)
        {
            return _context.hotels.Any(e => e.Hotel_ID == id);
        }
    }
}
public class HotelNameAndIdViewModel
{
    public int Hotel_ID { get; set; }
    public string Hotel_Name { get; set; }
}
