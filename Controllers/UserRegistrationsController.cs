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
using System.Net.Mail;
using System.Net;


namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistrationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;


        public UserRegistrationsController(DataContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;

        }

        // GET: api/UserRegistrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRegistration>>> GetUserRegistration()
        {
            return await _context.UserRegistration.ToListAsync();
        }

        [HttpGet("getCustomerForDropdown")]
        public async Task<ActionResult<IEnumerable<CustomerForDropdown>>> GetUserRegistrationCustomerForDropdown()
        {
            int roalId = (from r in await _context.UserRole.ToListAsync()
                          where r.Role_Name == "Customer"
                          select r.Role_ID).FirstOrDefault();
            List<UserRegistration> users = await _context.UserRegistration.ToListAsync();

            List<CustomerForDropdown> Customers = (from ur in await _context.RelationshipTB.ToListAsync()
                                                   where ur.Role_ID == roalId
                                                   select new CustomerForDropdown
                                                   {
                                                       User_ID = ur.User_ID,
                                                       Name = (from u in users
                                                               where u.User_ID == ur.User_ID
                                                               select u.First_Name +
                                                               " " + u.Last_Name).FirstOrDefault()
                                                   }).ToList();

            return Customers;
        }
        [HttpGet("getCustomerForEmail")]
        public async Task<ActionResult<IEnumerable<UserAndEmail>>> GetUserRegistrationCustomerForEmail()
        {
            int roalId = (from r in await _context.UserRole.ToListAsync()
                          where r.Role_Name == "Customer"
                          select r.Role_ID).FirstOrDefault();
            List<UserRegistration> users = await _context.UserRegistration.ToListAsync();

            List<UserAndEmail> Customers = (from ur in await _context.RelationshipTB.ToListAsync()
                                                   where ur.Role_ID == roalId
                                                   select new UserAndEmail
                                                   {
                                                       User_ID = ur.User_ID,
                                                       Name = (from u in users
                                                               where u.User_ID == ur.User_ID
                                                               select u.First_Name +
                                                               " " + u.Last_Name
                                                               ).FirstOrDefault(),
                                                        Email= (from u in users
                                                               where u.User_ID == ur.User_ID
                                                               select u.Email
                                                               ).FirstOrDefault(),
                                                   }).ToList();

            return Customers;
        }

        // GET: api/UserRegistrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRegistration>> GetUserRegistration(int id)
        {
            var userRegistration = await _context.UserRegistration.FindAsync(id);

            if (userRegistration == null)
            {
                return NotFound();
            }

            return userRegistration;
        }

        // PUT: api/UserRegistrations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRegistration(int id, UserRegistration userRegistration)
        {
            if (id != userRegistration.User_ID)
            {
                return BadRequest();
            }

            _context.Entry(userRegistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRegistrationExists(id))
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

        // POST: api/UserRegistrations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost("ForHotelOwner/{hid}")]

        public async Task<ActionResult<UserRegistration>> ForHotelOwner([FromForm] UserRegistrationCreateModel UserRegistrationCreateModel, int hid)
        {
            UserRegistration userRegistration = new UserRegistration { Active_Flag = UserRegistrationCreateModel.Active_Flag, ConatactNo = UserRegistrationCreateModel.ConatactNo, Delete_Flag = UserRegistrationCreateModel.Delete_Flag, DOB = UserRegistrationCreateModel.DOB, Email = UserRegistrationCreateModel.Email, First_Name = UserRegistrationCreateModel.First_Name, Gender = UserRegistrationCreateModel.Gender, Last_Name = UserRegistrationCreateModel.Last_Name, sortedfield = UserRegistrationCreateModel.sortedfield, State = UserRegistrationCreateModel.State };
            _context.UserRegistration.Add(userRegistration);
            await _context.SaveChangesAsync();

            if (UserRegistrationCreateModel.Profile_Image != null)
            {
                //foreach (IFormFile photo in UserRegistrationCreateModel.Profile_Image)
                {
                    String upfolder = Path.Combine(_env.ContentRootPath, "./Public/images/User");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + UserRegistrationCreateModel.Profile_Image.FileName;
                    String filePath = Path.Combine(upfolder, uniqueFileName);
                    using (var filesStrime = new FileStream(filePath, FileMode.Create))
                    {
                        UserRegistrationCreateModel.Profile_Image.CopyTo(filesStrime);
                    }

                    ImageMasterTB im = new ImageMasterTB
                    {
                        Image_URl = uniqueFileName,
                        Reference_ID = userRegistration.User_ID,
                        ReferenceTB_Name = "User",
                        Active_Flag = true,
                        Delete_Flag = false,
                        sortedfield = 99
                    };
                    _context.imageMasterTBs.Add(im);
                    await _context.SaveChangesAsync();
                    int rid = _context.UserRole
                        .Where(r => r.Role_Name == "HotelOwner")
                        .Select(r => r.Role_ID)
                        .FirstOrDefault();
                    RelationshipTB relationshipTB = new RelationshipTB
                    {
                        User_ID = userRegistration.User_ID,
                        Hotel_ID = hid,
                        Role_ID=rid
                        
                    };
                    _context.RelationshipTB.Add(relationshipTB);
                    await _context.SaveChangesAsync();
                    User user = new User
                    {
                        EmailID = userRegistration.Email,
                        User_ID = userRegistration.User_ID,
                        Password = "12345678"

                    };
                    _context.User.Add(user);

                    await _context.SaveChangesAsync();
                }

            }

            return CreatedAtAction("GetUserRegistration", new { id = userRegistration.User_ID }, userRegistration);
        }

        [HttpPost("ForHotelManager/{bid}")]

        public async Task<ActionResult<UserRegistration>> ForHotelManager([FromForm] UserRegistrationCreateModel UserRegistrationCreateModel, int bid)
        {
            UserRegistration userRegistration = new UserRegistration { Active_Flag = UserRegistrationCreateModel.Active_Flag, ConatactNo = UserRegistrationCreateModel.ConatactNo, Delete_Flag = UserRegistrationCreateModel.Delete_Flag, DOB = UserRegistrationCreateModel.DOB, Email = UserRegistrationCreateModel.Email, First_Name = UserRegistrationCreateModel.First_Name, Gender = UserRegistrationCreateModel.Gender, Last_Name = UserRegistrationCreateModel.Last_Name, sortedfield = UserRegistrationCreateModel.sortedfield, State = UserRegistrationCreateModel.State };
            _context.UserRegistration.Add(userRegistration);
            await _context.SaveChangesAsync();

            if (UserRegistrationCreateModel.Profile_Image != null)
            {
                //foreach (IFormFile photo in UserRegistrationCreateModel.Profile_Image)
                {
                    String upfolder = Path.Combine(_env.ContentRootPath, "./Public/images/User");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + UserRegistrationCreateModel.Profile_Image.FileName;
                    String filePath = Path.Combine(upfolder, uniqueFileName);
                    using (var filesStrime = new FileStream(filePath, FileMode.Create))
                    {
                        UserRegistrationCreateModel.Profile_Image.CopyTo(filesStrime);
                    }

                    ImageMasterTB im = new ImageMasterTB
                    {
                        Image_URl = uniqueFileName,
                        Reference_ID = userRegistration.User_ID,
                        ReferenceTB_Name = "User",
                        Active_Flag = true,
                        Delete_Flag = false,
                        sortedfield = 99
                    };
                    _context.imageMasterTBs.Add(im);
                    await _context.SaveChangesAsync();
                    int rid = _context.UserRole
                        .Where(r => r.Role_Name == "HotelManager")
                        .Select(r => r.Role_ID)
                        .FirstOrDefault();
                    RelationshipTB relationshipTB = new RelationshipTB
                    {
                        User_ID = userRegistration.User_ID,
                        Branch_ID = bid,
                        Role_ID = rid

                    };
                    _context.RelationshipTB.Add(relationshipTB);
                    await _context.SaveChangesAsync();
                    User user = new User
                    {
                        EmailID = userRegistration.Email,
                        User_ID = userRegistration.User_ID,
                        Password = "12345678"

                    };
                    _context.User.Add(user);

                    await _context.SaveChangesAsync();
                }

            }

            return CreatedAtAction("GetUserRegistration", new { id = userRegistration.User_ID }, userRegistration);
        }

        [HttpPost("ForHotelReceptionist/{bid}")]
        public async Task<ActionResult<UserRegistration>> ForHotelReceptionist([FromForm] UserRegistrationCreateModel UserRegistrationCreateModel, int bid)
        {
            UserRegistration userRegistration = new UserRegistration { Active_Flag = UserRegistrationCreateModel.Active_Flag, ConatactNo = UserRegistrationCreateModel.ConatactNo, Delete_Flag = UserRegistrationCreateModel.Delete_Flag, DOB = UserRegistrationCreateModel.DOB, Email = UserRegistrationCreateModel.Email, First_Name = UserRegistrationCreateModel.First_Name, Gender = UserRegistrationCreateModel.Gender, Last_Name = UserRegistrationCreateModel.Last_Name, sortedfield = UserRegistrationCreateModel.sortedfield, State = UserRegistrationCreateModel.State };
            _context.UserRegistration.Add(userRegistration);
            await _context.SaveChangesAsync();

            if (UserRegistrationCreateModel.Profile_Image != null)
            {
                //foreach (IFormFile photo in UserRegistrationCreateModel.Profile_Image)
                {
                    String upfolder = Path.Combine(_env.ContentRootPath, "./Public/images/User");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + UserRegistrationCreateModel.Profile_Image.FileName;
                    String filePath = Path.Combine(upfolder, uniqueFileName);
                    using (var filesStrime = new FileStream(filePath, FileMode.Create))
                    {
                        UserRegistrationCreateModel.Profile_Image.CopyTo(filesStrime);
                    }

                    ImageMasterTB im = new ImageMasterTB
                    {
                        Image_URl = uniqueFileName,
                        Reference_ID = userRegistration.User_ID,
                        ReferenceTB_Name = "User",
                        Active_Flag = true,
                        Delete_Flag = false,
                        sortedfield = 99
                    };
                    _context.imageMasterTBs.Add(im);
                    await _context.SaveChangesAsync();
                    int rid = _context.UserRole
                        .Where(r => r.Role_Name == "HotelReceptionist")
                        .Select(r => r.Role_ID)
                        .FirstOrDefault();
                    RelationshipTB relationshipTB = new RelationshipTB
                    {
                        User_ID = userRegistration.User_ID,
                        Branch_ID = bid,
                        Role_ID = rid

                    };
                    _context.RelationshipTB.Add(relationshipTB);

                    User user = new User
                    {
                        EmailID = userRegistration.Email,
                        User_ID = userRegistration.User_ID,
                        Password = "12345678"

                    };
                    _context.User.Add(user);

                    await _context.SaveChangesAsync();
                }

            }

            return CreatedAtAction("GetUserRegistration", new { id = userRegistration.User_ID }, userRegistration);
        }



        [HttpPost]

        public async Task<ActionResult<UserRegistration>> PostUserRegistration([FromForm]UserRegistrationCreateModel UserRegistrationCreateModel)
        {
            UserRegistration userRegistration = new UserRegistration { Active_Flag = UserRegistrationCreateModel.Active_Flag, ConatactNo = UserRegistrationCreateModel.ConatactNo, Delete_Flag = UserRegistrationCreateModel.Delete_Flag, DOB = UserRegistrationCreateModel.DOB, Email = UserRegistrationCreateModel.Email, First_Name = UserRegistrationCreateModel.First_Name, Gender = UserRegistrationCreateModel.Gender, Last_Name = UserRegistrationCreateModel.Last_Name, sortedfield = UserRegistrationCreateModel.sortedfield, State = UserRegistrationCreateModel.State };
            _context.UserRegistration.Add(userRegistration);
            await _context.SaveChangesAsync();

            if (UserRegistrationCreateModel.Profile_Image != null)
            {
                //foreach (IFormFile photo in UserRegistrationCreateModel.Profile_Image)
                {
                    String upfolder = Path.Combine(_env.ContentRootPath, "./Public/images/User");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + UserRegistrationCreateModel.Profile_Image.FileName;
                    String filePath = Path.Combine(upfolder, uniqueFileName);
                    using (var filesStrime = new FileStream(filePath, FileMode.Create))
                    {
                        UserRegistrationCreateModel.Profile_Image.CopyTo(filesStrime);
                    }

                    ImageMasterTB im = new ImageMasterTB
                    {
                        Image_URl = uniqueFileName,
                        Reference_ID = UserRegistrationCreateModel.User_ID,
                        ReferenceTB_Name = "User",
                        Active_Flag = true,
                        Delete_Flag = false,
                        sortedfield = 99
                    };
                    _context.imageMasterTBs.Add(im);
                    await _context.SaveChangesAsync();
                    int rid = _context.UserRole
                             .Where(r => r.Role_Name == "Customer")
                             .Select(r => r.Role_ID)
                              .FirstOrDefault();
                    RelationshipTB relationshipTB = new RelationshipTB
                    {
                        User_ID = userRegistration.User_ID,
                        Role_ID = rid

                    };
                    _context.RelationshipTB.Add(relationshipTB);
                    await _context.SaveChangesAsync();

                }

            }

            return CreatedAtAction("GetUserRegistration", new { id = userRegistration.User_ID }, userRegistration);
        }


        // DELETE: api/UserRegistrations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserRegistration>> DeleteUserRegistration(int id)
        {
            var userRegistration = await _context.UserRegistration.FindAsync(id);
            if (userRegistration == null)
            {
                return NotFound();
            }

            _context.UserRegistration.Remove(userRegistration);
            await _context.SaveChangesAsync();

            return userRegistration;
        }


   



        private bool UserRegistrationExists(int id)
        {
            return _context.UserRegistration.Any(e => e.User_ID == id);
        }
    }
}
