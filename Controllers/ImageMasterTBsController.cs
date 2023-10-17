using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Managment_API.DBContext;
using Hotel_Managment_API.Models;
using Hotel_Managment_API.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageMasterTBsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public ImageMasterTBsController(DataContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }

        // GET: api/ImageMasterTBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageMasterTB>>> GetimageMasterTBs()
        {
            return await _context.imageMasterTBs.ToListAsync();
        }

        // GET: api/ImageMasterTBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageMasterTB>> GetImageMasterTB(int id)
        {
            var imageMasterTB = await _context.imageMasterTBs.FindAsync(id);

            if (imageMasterTB == null)
            {
                return NotFound();
            }

            return imageMasterTB;
        }

        // PUT: api/ImageMasterTBs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageMasterTB(int id, ImageMasterTB imageMasterTB)
        {
            if (id != imageMasterTB.Image_ID)
            {
                return BadRequest();
            }

            _context.Entry(imageMasterTB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageMasterTBExists(id))
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

        // POST: api/ImageMasterTBs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ImageMasterTB>> PostImageMasterTB([FromForm] ImageViewModel imageMasterViewTB)
        {
            if (ModelState.IsValid)
            {
                string uniquefilename = null;
                if (imageMasterViewTB.Image_URl != null)
                {
                    string uploadsfolder = Path.Combine(_env.ContentRootPath, "./Public/images/hotels");

                    uniquefilename = Guid.NewGuid().ToString() + "_" + imageMasterViewTB.Reference_ID + "_" + imageMasterViewTB.Image_URl.FileName;
                    string filepath = Path.Combine(uploadsfolder, uniquefilename);

                    imageMasterViewTB.Image_URl.CopyTo(new FileStream(filepath, FileMode.Create));

                }
                ImageMasterTB im = new ImageMasterTB
                {
                    Image_URl = uniquefilename,
                    Reference_ID = imageMasterViewTB.Reference_ID,
                    ReferenceTB_Name = imageMasterViewTB.ReferenceTB_Name,
                    Active_Flag = imageMasterViewTB.Active_Flag,
                    Delete_Flag = imageMasterViewTB.Delete_Flag,
                    sortedfield = imageMasterViewTB.SortedFiled
                };
               
            _context.imageMasterTBs.Add(im);
            await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetImageMasterTB", new { id = imageMasterViewTB.Image_ID }, imageMasterViewTB);
        }

        // DELETE: api/ImageMasterTBs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageMasterTB>> DeleteImageMasterTB(int id)
        {
            var imageMasterTB = await _context.imageMasterTBs.FindAsync(id);
            if (imageMasterTB == null)
            {
                return NotFound();
            }

            _context.imageMasterTBs.Remove(imageMasterTB);
            await _context.SaveChangesAsync();

            return imageMasterTB;
        }

        private bool ImageMasterTBExists(int id)
        {
            return _context.imageMasterTBs.Any(e => e.Image_ID == id);
        }
    }
}
