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
using Hotel_Managment_API.ViewModels;
using System.Net.Mail;
using System.Net;

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

        [HttpPost("SendEmail")]
        public async Task<ActionResult<Coupon>> SendEmail(ReqSendCoupen reqSendCoupen)
        {
            List<Coupon> coupons = (from c in await _context.Coupon.ToListAsync()
                                    where c.Hotel_ID == reqSendCoupen.hid && c.Coupon_Name == reqSendCoupen.Cname && c.Assign_Flag == false
                                    select c).ToList();

            List<UserRegistration> users = (await _context.UserRegistration.ToListAsync()).Where(user => reqSendCoupen.userIds.Contains(user.User_ID)).ToList();

            if (coupons.Count >= users.Count)
            {
                int index = 0;

                foreach(Coupon c in coupons)
                {
                    if (index == users.Count)
                    {
                        break;
                    }
                    c.Assign_Flag = true;
                    c.Assign_UId = users[index].User_ID;
                        

                   // var client = new SmtpClient("smtp-mail.gmail.com", 587)
                   // {
                   //     EnableSsl = true,
                   //     //UseDefaultCredentials = false,
                   //     Credentials = new NetworkCredential("mazzking666@gmail.com", "xctj naln sjnj gjsv")
                   // };

                   //await client.SendMailAsync(
                   // new MailMessage(from: "mazzking666@gmail.com",
                   // to: users[index].Email,
                   // "you get coupon ",
                   // $@"Your {c.Discount_Percentage} %  Discount Coupon Code :- {c.Coupon_Code} It's  Start_Date:- {c.Start_Date}Expiry_Date :- {c.Expiry_Date}"
                   // ));

                    try
                    {
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                        {
                            EnableSsl = true,
                            Credentials = new NetworkCredential("mazzking666@gmail.com", "xctj naln sjnj gjsv"),
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Timeout = 20000, // Adjust the timeout as needed
                        };

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress("mazzking666@gmail.com"),
                            Subject = "You've got a coupon!",
                            Body = $"Your {c.Discount_Percentage}% Discount Coupon Code: {c.Coupon_Code}. Start Date: {c.Start_Date}, Expiry Date: {c.Expiry_Date}",
                            IsBodyHtml = false, // You can set this to true if you want to send HTML content
                        };

                        mailMessage.To.Add(users[index].Email);

                        await smtpClient.SendMailAsync(mailMessage);
                        index++;
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (log, etc.)
                        Console.WriteLine($"Error sending email: {ex.Message}");
                    }

                    //user: 'mazzking666@gmail.com',  
                    //pass: 'xctj naln sjnj gjsv',
                    
                }
            }

            return CreatedAtAction("SendCoupon", new {  });
        }

        // GET: api/Coupons
        [HttpGet("ByHotelID/{id}")]
        public async Task<ActionResult<IEnumerable<couponViewModelForIndex>>> GetCouponByHotelId(int id)
        {
            List<Coupon> data = (from c in await _context.Coupon.ToListAsync()
                                   where c.Hotel_ID == id && c.Assign_Flag == false
                                   select c).ToList();

            List<couponViewModelForIndex> Cdata = data
        .GroupBy(c => c.Coupon_Name)
        .Select(group => new couponViewModelForIndex
        {
            Coupon_Name = group.Key,
            Start_Date = group.First().Start_Date,
            Expiry_Date = group.First().Expiry_Date,
            Discount_Percentage = group.First().Discount_Percentage,
            noOfAvailableCoupen = group.Count()
        })
        .ToList();

            return Cdata;
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
            if (coupon.Start_Date.Date > coupon.Expiry_Date.Date || coupon.Expiry_Date.Date < DateTime.Now.Date || coupon.Start_Date.Date < DateTime.Now.Date)
            {
                return BadRequest(new { Errormessage = "Invalid start / Expiry date" });

            }
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
                    Start_Date=coupon.Start_Date,
                    Assign_Flag=false,
                    Assign_UId=0,
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
