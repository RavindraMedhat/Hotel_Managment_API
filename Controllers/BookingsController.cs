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
using System.Net.Mail;
using System.Net;

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
        public async Task<ActionResult<BookingForDetail>> GetBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            UserRegistration userRegistration = await _context.UserRegistration.FindAsync(booking.User_ID);
            HotelBranchTB hotelBranchTB = await _context.hotelBranchTBs.FindAsync(booking.Branch_ID);
            HotelTB hotelTB = await _context.hotels.FindAsync(hotelBranchTB.Hotel_ID);
            RoomTB room = await _context.RoomTB.FindAsync(booking.Room_ID);
            RoomCategoryTB roomCategoryTB = await _context.roomCategoryTB.FindAsync(room.Category_ID);


            BookingForDetail bookingForDetail = new BookingForDetail()
            {
                Booking_Date = booking.Booking_Date,
                Booking_Status = booking.Booking_Status,
                Branch_name = hotelBranchTB.Branch_Name,
                Check_In_Date = booking.Check_In_Date,
                Check_Out_Date = booking.Check_Out_Date,
                Coupon_Code = booking.Coupon_Code,
                Customer_Name = userRegistration.First_Name + " " + userRegistration.Last_Name,
                Customer_status = booking.Customer_status,
                Discount = booking.Discount,
                Group_ID = booking.Group_ID,
                Hotel_name = hotelTB.Hotel_Name,
                Payed_Amount = booking.Payed_Amount,
                Payment_Mode = booking.Payment_Mode,
                Payment_Status = booking.Payment_Status,
                Room_No = roomCategoryTB.Category_Name + " " + room.Room_No

            };

            return bookingForDetail;
        }

        [HttpGet("getBookingByUid/{id}")]
        public async Task<ActionResult<List<BookingForDetail>>> GetBookingByUid(int id)
        {
            List<Booking> booking = await _context.Booking.ToListAsync();

            booking = booking.Where(b => b.User_ID == id).ToList();

            if (booking == null)
            {
                return NotFound();
            }

            List<UserRegistration> userRegistration = await _context.UserRegistration.ToListAsync();
            List<HotelBranchTB> hotelBranchTB = await _context.hotelBranchTBs.ToListAsync();
            List<HotelTB> hotelTB = await _context.hotels.ToListAsync();
            List<RoomTB> room = await _context.RoomTB.ToListAsync();
            List<RoomCategoryTB> roomCategoryTB = await _context.roomCategoryTB.ToListAsync();


            List<BookingForDetail> bookingForDetail = (from b in booking
                                                       select new BookingForDetail()
                                                       {
                                                           booking_id = b.Booking_ID,
                                                           Booking_Date = b.Booking_Date,
                                                           Booking_Status = b.Booking_Status,

                                                           Branch_name = (from hb in hotelBranchTB
                                                                          where hb.Branch_ID == b.Branch_ID
                                                                          select hb.Branch_Name).FirstOrDefault(),

                                                           Check_In_Date = b.Check_In_Date,
                                                           Check_Out_Date = b.Check_Out_Date,
                                                           Coupon_Code = b.Coupon_Code,

                                                           Customer_Name = (from u in userRegistration
                                                                            where u.User_ID == b.User_ID
                                                                            select u.First_Name + " " + u.Last_Name).FirstOrDefault(),

                                                           Customer_status = b.Customer_status,
                                                           Discount = b.Discount,
                                                           Group_ID = b.Group_ID,

                                                           Hotel_name = (from h in hotelTB
                                                                         where h.Hotel_ID == (from hb in hotelBranchTB
                                                                                              where hb.Branch_ID == b.Branch_ID
                                                                                              select hb.Hotel_ID).FirstOrDefault()
                                                                         select h.Hotel_Name).FirstOrDefault(),

                                                           Payed_Amount = b.Payed_Amount,
                                                           Payment_Mode = b.Payment_Mode,
                                                           Payment_Status = b.Payment_Status,

                                                           Room_No = (from r in room
                                                                      where r.Room_ID == b.Room_ID
                                                                      select r.Room_No).FirstOrDefault() + " " + (from r in room
                                                                                                                  where r.Room_ID == b.Room_ID
                                                                                                                  select(from rc in roomCategoryTB
                                                                                                                   where rc.Category_ID == r.Category_ID
                                                                                                                   select rc.Category_Name).FirstOrDefault()).FirstOrDefault(),
                                                       }).ToList();

            return bookingForDetail;
        }


        [HttpPost("ByRoomIDAndDate")]
        public async Task<ActionResult<List<ViewModelForAvailability>>> GetBookingByRoomIDAndDate(reqDataForAvailability data)
        {
            List<Booking> booking = (from b in await _context.Booking.ToListAsync()
                                     where b.Room_ID == data.Room_id
                                     select b).ToList();
            List<ViewModelForAvailability> finalData = new List<ViewModelForAvailability>();


            for (int i = 0; i < 12; i++)
            {
                Booking book = booking.Where(b => b.Check_In_Date.Date <= data.date.Date && b.Check_Out_Date.Date >= data.date.Date).FirstOrDefault();

                if (book != null)
                {
                    UserRegistration customer = await _context.UserRegistration.FindAsync(book.User_ID);

                    finalData.Add(new ViewModelForAvailability { Availability = "Booked", Booking_ID = book.Booking_ID, Date = data.date, Room_ID = book.Room_ID, Customer_Name = customer.First_Name + " " + customer.Last_Name, Customer_Id = customer.User_ID });
                }
                else
                {
                    finalData.Add(new ViewModelForAvailability { Availability = "Available", Booking_ID = 0, Date = data.date, Room_ID = 0, Customer_Name = "-", Customer_Id = 0 });
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
            booking.Active_Flag = true;
            booking.Delete_Flag = false;
            booking.Sortedfield = 99;

            if (booking.Coupon_Code != null)
            {
                int hid = (await _context.hotelBranchTBs.FindAsync(booking.Branch_ID)).Hotel_ID;


                List<Coupon> coupons = (from c in await _context.Coupon.ToListAsync()
                                        where c.Coupon_Code == booking.Coupon_Code && c.Start_Date.Date <= DateTime.Now.Date && c.Expiry_Date.Date >= DateTime.Now.Date && c.Hotel_ID == hid
                                        select c).ToList();
                if (coupons.Count == 0)
                {
                    return BadRequest(new { Errormessage = "ianvlid coupon code" });
                }
            }


            if (booking.Check_In_Date.Date > booking.Check_Out_Date.Date || booking.Check_Out_Date.Date < DateTime.Now.Date || booking.Check_In_Date.Date < DateTime.Now.Date)
            {
                return BadRequest(new { Errormessage = "Invalid Check in / check out date" });

            }

            List<Booking> bookings = (from b in await _context.Booking.ToListAsync()
                                      where b.Room_ID == booking.Room_ID
                                      select b).ToList();

            DateTime ErrorDate = DateTime.Now;
            bool IsError = false;
            foreach (Booking b in bookings)
            {
                DateTime Check_In_Date = booking.Check_In_Date;
                DateTime Check_Out_Date = booking.Check_Out_Date;

                while (Check_In_Date <= Check_Out_Date)
                {
                    ErrorDate = Check_In_Date;
                    if (Check_In_Date.Date == b.Check_In_Date.Date || Check_In_Date.Date == b.Check_Out_Date)
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
                return BadRequest(new { Errormessage = ErrorDate.Date.ToString() + "  is not available" });
            }

            booking.Group_ID = booking.User_ID + "_" + booking.Branch_ID + "_" + DateTime.Now.Date;

            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            try
            {
                UserRegistration user = await _context.UserRegistration.FindAsync(booking.User_ID);

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
                    Subject = "Your Room Booked",
                    Body = $"Hey, {user.First_Name} {user.Last_Name} \n your room is booked for {booking.Check_In_Date.Date} to {booking.Check_Out_Date.Date} your Booking id :- {booking.Booking_ID}",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(user.Email);
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, etc.)
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

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
