using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Entities;
using System.Web.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MilkTea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly MilkTeaContext _context;
        public IConfiguration _configuration;

        public AdminsController(MilkTeaContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // PUT: api/Admins/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest();
            }

            admin.Password = Crypto.HashPassword(admin.Password);
            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        
        // POST: api/Admins/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Admin admin)
        {
            if (admin != null && admin.UserName != null && admin.Password != null)
            {
                var userExist = await GetUser(admin.UserName);

                if (userExist != null)
                {
                    if (Crypto.VerifyHashedPassword(userExist.Password, admin.Password))
                    {
                        //Create claims details based on the user information
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("AdminId", userExist.AdminId.ToString()),
                            new Claim("UserName", userExist.UserName),
                            new Claim("Password", userExist.Password)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var _token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                        var token = new JwtSecurityTokenHandler().WriteToken(_token);
                        var resultAdmin = new { userExist.AdminId, userExist.UserName, userExist.FullName };
                        return Ok(new { token, resultAdmin });
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

        private async Task<Admin> GetUser(string username)
        {
            return await _context.Admins.FirstOrDefaultAsync(u => u.UserName == username);
        }

        // POST: api/Admins
        [HttpPost]
        public ActionResult<Admin> PostAdmin(Admin admin)
        {
            admin.Password = Crypto.HashPassword(admin.Password);
            if (UserNameExists(admin.UserName))
            {
                return BadRequest("Username already exists!");
            }
            _context.Admins.Add(admin);
            _context.SaveChanges();
            return CreatedAtAction("GetAdmin", new { id = admin.AdminId }, admin);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }

        private bool UserNameExists(string username)
        {
            return _context.Admins.Any(e => e.UserName == username);
        }
    }
}
