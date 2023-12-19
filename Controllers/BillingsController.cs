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

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingsController : ControllerBase
    {
        private readonly DataContext _context;

        public BillingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Billings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Billing>>> GetBilling()
        {
            return await _context.Billing.ToListAsync();
        }

        // GET: api/Billings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> GetBilling(int id)
        {
            var billing = await _context.Billing.FindAsync(id);

            if (billing == null)
            {
                return NotFound();
            }

            return billing;
        }

        // PUT: api/Billings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBilling(int id, Billing billing)
        {
            if (id != billing.Bill_ID)
            {
                return BadRequest();
            }

            _context.Entry(billing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillingExists(id))
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

        // POST: api/Billings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        [HttpPost("{Gid}")]
        public async Task<ActionResult<Billing>> PostBilling(string Gid)
        {
            List<Booking> bookings = (from b in await _context.Booking.ToListAsync()
                                     where b.Group_ID == Gid 
                                     select b).ToList();
            if (bookings.Count==0)
            {
                return BadRequest(new { Errormessage = "No booking found " });
            }

            Billing billing = new Billing();
            billing.Group_ID = Gid;
            List<Details> details = new List<Details>();

            List<RoomTB> roomTBs = (from r in await _context.RoomTB.ToListAsync()
                                   select r).ToList();

            List<HotelBranchTB> hotelBranchTBs = await _context.hotelBranchTBs.ToListAsync();

            foreach (Booking b in bookings)
            {
                foreach(RoomTB r in roomTBs)
                {
                    if(r.Room_ID == b.Room_ID)
                    {

                        billing.Total_Amount = billing.Total_Amount +( r.Room_Price * (b.Check_Out_Date - b.Check_In_Date).Days ) ;
                        details.Add(new Details() {detail = r.Room_No +" * "+((b.Check_Out_Date -b.Check_In_Date).Days+1) +" days",amount= r.Room_Price *(1+ (b.Check_Out_Date - b.Check_In_Date).Days) });

                        if(b.Payed_Amount != 0)
                        {
                            billing.Payed_Amount = billing.Payed_Amount + b.Payed_Amount;
                            details.Add(new Details() {detail= "Payed amount - ",amount= b.Payed_Amount });
                        }

                        if (b.Coupon_Code != null)
                        {
                            Coupon c = (await _context.Coupon.ToListAsync()).Where(c=>c.Coupon_Code == b.Coupon_Code).FirstOrDefault();

                            billing.Discount_Amount = billing.Discount_Amount +  r.Room_Price * c.Discount_Percentage * (1 + (b.Check_Out_Date - b.Check_In_Date).Days) / 100;

                            details.Add(new Details() { detail = c.Discount_Percentage + "% Discount of Coupon Code :-  " + c.Coupon_Code, amount = r.Room_Price * c.Discount_Percentage * (1 + (b.Check_Out_Date - b.Check_In_Date).Days) / 100 });

                        }
                        else
                        {
                            List<Discount> discounts = (from d in await                                                       _context.Discount.ToListAsync()
                                                        where d.Hotel_ID == r.Hotel_ID &&                                     d.Start_Date <= b.Booking_Date &&
                                                        d.End_Date>= b.Booking_Date
                                                        select d).ToList();
                            if(discounts.Count > 0)
                            {
                                Discount d = discounts.First();
                                billing.Discount_Amount = billing.Discount_Amount +                                             r.Room_Price *                                                        d.Discount_Percentage * (1 + (b.Check_Out_Date - b.Check_In_Date).Days) / 100;

                                details.Add(new Details() { detail = d.Discount_Percentage + "% Discount of " + d.Discount_Name, amount = r.Room_Price * d.Discount_Percentage * (1 + (b.Check_Out_Date - b.Check_In_Date).Days) / 100 });
                            }
                        }
                    }
                }
            }

            billing.Final_Amount = billing.Total_Amount - billing.Discount_Amount - billing.Payed_Amount ;

            billing.Bill_Date = DateTime.Now;

            Billing bill = (await _context.Billing.ToListAsync()).Where(b => b.Group_ID == billing.Group_ID).FirstOrDefault();

            if (bill==null)
            {
                _context.Billing.Add(billing);
                await _context.SaveChangesAsync();
            }
            else
            {
                billing.Bill_ID = bill.Bill_ID;
                billing.Bill_Date = bill.Bill_Date;
            }

            BillRes billRes = new BillRes();
            billRes.bill = billing;
            billRes.details = details;
            return CreatedAtAction("GetBilling", new { id = billing.Bill_ID }, billRes);
        }

        // DELETE: api/Billings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Billing>> DeleteBilling(int id)
        {
            var billing = await _context.Billing.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }

            _context.Billing.Remove(billing);
            await _context.SaveChangesAsync();

            return billing;
        }

        private bool BillingExists(int id)
        {
            return _context.Billing.Any(e => e.Bill_ID == id);
        }
    }
}
