using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Managment_API.DBContext;
using Hotel_Managment_API.Models;

namespace Hotel_Managment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login( LoginRequest loginrequest)
        {
            LoginResponse loginResponse = new LoginResponse
            {
                Success = false,
                Message = "",
                RedirctID = 0,
                Redirect = "",
                Role = ""
            };

            User user = (from u in await _context.User.ToListAsync()
                         where u.EmailID == loginrequest.EmailID && u.Password == loginrequest.Password
                         select u).FirstOrDefault();
            if (user == null)
            {
                User userc = (from u in await _context.User.ToListAsync()
                              where u.EmailID == loginrequest.EmailID
                              select u).FirstOrDefault();
                loginResponse.Success = false;
                if (userc == null)
                {
                    loginResponse.Message = "Chack Email";
                }
                else
                {
                    loginResponse.Message = "Chack Password";
                }
            }
            else
            {
                loginResponse.Success = true;
                loginResponse.Message = "";

                RelationshipTB userr = (from r in await _context.RelationshipTB.ToListAsync()
                                        where r.User_ID == user.User_ID
                                        select r
                                        ).FirstOrDefault();
                UserRole userRole = (from ur in await _context.UserRole.ToListAsync()
                                     where ur.Role_ID == userr.Role_ID
                                     select ur).FirstOrDefault();
                if (userRole.Role_Name == "SuperAdmin")
                {
                    loginResponse.Redirect = "Hotel";
                    
                }
                else if (userRole.Role_Name == "HotelOwner")
                {
                    loginResponse.Redirect = "HotelBranch";
                    loginResponse.RedirctID = userr.Hotel_ID;
                }
                else if (userRole.Role_Name == "HotelManager")
                {
                    loginResponse.Redirect = "Room";
                    loginResponse.RedirctID = userr.Branch_ID;
                }
                else if (userRole.Role_Name == "HotelReceptionist")
                {
                    loginResponse.Redirect = "Room";
                    loginResponse.RedirctID = userr.Branch_ID;
                }
                loginResponse.Role = userRole.Role_Name;
            }

            return loginResponse;




        }
        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.EmailID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.EmailID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.EmailID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.EmailID == id);
        }
    }
}
