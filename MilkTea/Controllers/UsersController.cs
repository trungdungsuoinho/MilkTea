using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MilkTea.Entities;

namespace MilkTea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MilkTeaContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(MilkTeaContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            user.UserName = _context.Entry(user).GetDatabaseValues().GetValue<string>("UserName");
            user.VerifyPhone = _context.Entry(user).GetDatabaseValues().GetValue<bool>("VerifyPhone");
            if (user.Email == _context.Entry(user).GetDatabaseValues().GetValue<string>("Email"))
            {
                user.VerifyEmail = _context.Entry(user).GetDatabaseValues().GetValue<bool>("VerifyEmail");
            }
            else
            {
                user.VerifyEmail = false;
            }
            if (user.Password != null)
            {
                user.Password = Crypto.HashPassword(user.Password);
            }
            else
            {
                user.Password = _context.Entry(user).GetDatabaseValues().GetValue<string>("Password");
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
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

        // PUT: api/Users/5
        [HttpPut("Password")]
        public IActionResult PutPassword(User _user)
        {
            User user = GetUser(_user.UserName);
            if (user == null)
            {
                return NotFound();
            }
            if (_user.Password != null)
            {
                user.Password = Crypto.HashPassword(_user.Password);
            }
            else
            {
                user.Password = _context.Entry(user).GetDatabaseValues().GetValue<string>("Password");
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost("Login")]
        public IActionResult Login(User _user)
        {
            if (_user != null && _user.UserName != null && _user.Password != null)
            {
                var userExist = GetUser(_user.UserName);
                if (userExist != null)
                {
                    if (Crypto.VerifyHashedPassword(userExist.Password, _user.Password))
                    {
                        //Create claims details based on the user information
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", userExist.UserId.ToString()),
                            new Claim("UserName", userExist.UserName)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var _token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                        var token = new JwtSecurityTokenHandler().WriteToken(_token);
                        var user = new { userExist.UserId, userExist.UserName, userExist.FullName };
                        return Ok(new { token, user });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private User GetUser(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult<User> PostUser(User user)
        {
            user.Password = Crypto.HashPassword(user.Password);
            if (UserNameExists(user.UserName))
            {
                return BadRequest();
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        private bool UserNameExists(string username)
        {
            return _context.Users.Any(e => e.UserName == username);
        }
    }
}
