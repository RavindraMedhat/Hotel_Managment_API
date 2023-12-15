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
    public class BookingsController : ControllerBase
    {
        private readonly DataContext _context;

        public BookingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            return await _context.Booking.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpPost("ByRoomIDAndDate")]
        public async Task<ActionResult<List<ViewModelForAvailability>>> GetBookingByRoomIDAndDate(reqDataForAvailability data)
        {
            List<Booking> booking = (from b in await _context.Booking.ToListAsync()
                                     where b.Room_ID == data.Room_id
                                     select b).ToList();
            List<ViewModelForAvailability> finalData = new List<ViewModelForAvailability>();


            for(int i = 0; i < 10; i++)
            {
                Booking book = booking.Where(b => b.Check_In_Date.Date <= data.date.Date && b.Check_Out_Date.Date >= data.date.Date).FirstOrDefault();

                if (book!=null)
                {
                    UserRegistration customer = await _context.UserRegistration.FindAsync(book.User_ID);

                    finalData.Add(new ViewModelForAvailability { Availability = "Booked", Booking_ID = book.Booking_ID, Date = data.date, Room_ID = book.Room_ID, Customer_Name = customer.First_Name+" "+customer.Last_Name });
                }
                else
                {
                    finalData.Add(new ViewModelForAvailability { Availability = "Available", Booking_ID = 0, Date = data.date, Room_ID = 0, Customer_Name = "-" });
                }

               data.date = data.date.AddDays(1);
            }



            if (finalData == null)
            {
                return NotFound();
            }

            return finalData;
        }


        // PUT: api/Bookings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Booking_ID)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            List<Booking> bookings = (from b in await _context.Booking.ToListAsync()
                                     where b.Room_ID == booking.Room_ID
                                     select b).ToList();
            
            DateTime ErrorDate = DateTime.Now ;
            bool IsError = false;
            foreach(Booking b in bookings)
            {
                DateTime Check_In_Date = booking.Check_In_Date;
                DateTime Check_Out_Date = booking.Check_Out_Date;

                while (Check_In_Date <= Check_Out_Date)
                {
                    ErrorDate = Check_In_Date;
                    if(Check_In_Date.Date == b.Check_In_Date.Date || Check_In_Date.Date == b.Check_Out_Date)
                    {
                        IsError = true;
                        break;
                    }
                    Check_In_Date = Check_In_Date.AddDays(1);
                }

                if (IsError)
                {
                    break;
                }
            };

            if (IsError)
            {
                return BadRequest(new { Errormessage = ErrorDate.Date.ToString()+"  is not available" });
            }

            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Booking_ID }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Booking>> DeleteBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Booking_ID == id);
        }
    }
}
