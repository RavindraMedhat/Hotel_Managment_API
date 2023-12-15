using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Managment_API.DBContext;
using Hotel_Managment_API.Models;
using System.Text;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly DataContext _context;

        public CouponsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Coupons
        [HttpGet("ByHotelID/{id}")]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetCouponByHotelId(int id)
        {
            List<Coupon> data = (from c in await _context.Coupon.ToListAsync()
                                   where c.Hotel_ID == id
                                   select c).ToList();
            return data;
           // return await _context.Coupon.ToListAsync();
        }

        // GET: api/Coupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coupon>> GetCoupon(int id)
        {
            var coupon = await _context.Coupon.FindAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            return coupon;
        }

        // PUT: api/Coupons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoupon(int id, Coupon coupon)
        {
            if (id != coupon.Coupon_ID)
            {
                return BadRequest();
            }

            _context.Entry(coupon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
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

        // POST: api/Coupons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Coupon>> PostCoupon(CouponViewModelForCreate coupon)
        {
            string GenerateRandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                StringBuilder stringBuilder = new StringBuilder(length);

                for (int i = 0; i < length; i++)
                {
                    int index = random.Next(chars.Length);
                    stringBuilder.Append(chars[index]);
                }

                return stringBuilder.ToString();
            }

            for(int i = 0; i < coupon.Number_Coupon; i++)
            {
                Coupon NewCoupon = new Coupon
                {
                    Coupon_Code = GenerateRandomString(6),
                    Active_Flag=coupon.Active_Flag,
                    Coupon_Name=coupon.Coupon_Name,
                    Delete_Flag=coupon.Delete_Flag,
                    Discount_Percentage=coupon.Discount_Percentage,
                    Expiry_Date=coupon.Expiry_Date,
                    Hotel_ID=coupon.Hotel_ID,
                    Sortedfield=coupon.Sortedfield,
                    Start_Date=coupon.Start_Date
                };
            _context.Coupon.Add(NewCoupon);
            
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoupon", new { id = coupon.Coupon_ID }, coupon);
        }

        // DELETE: api/Coupons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Coupon>> DeleteCoupon(int id)
        {
            var coupon = await _context.Coupon.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            _context.Coupon.Remove(coupon);
            await _context.SaveChangesAsync();

            return coupon;
        }

        private bool CouponExists(int id)
        {
            return _context.Coupon.Any(e => e.Coupon_ID == id);
        }
    }
}
