using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Entities;

namespace MilkTea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivesController : ControllerBase
    {
        private readonly MilkTeaContext _context;

        public ReceivesController(MilkTeaContext context)
        {
            _context = context;
        }

        // GET: api/Receives
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receive>>> GetReceives()
        {
            return await _context.Receives.ToListAsync();
        }

        // GET: api/Receives/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Receive>> GetReceive(int id)
        {
            var receive = await _context.Receives.FindAsync(id);

            if (receive == null)
            {
                return NotFound();
            }

            return receive;
        }

        // PUT: api/Receives/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceive(int id, Receive receive)
        {
            if (id != receive.ReceiveId)
            {
                return BadRequest();
            }

            _context.Entry(receive).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiveExists(id))
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

        // POST: api/Receives
        [HttpPost]
        public async Task<ActionResult<Receive>> PostReceive(Receive receive)
        {
            _context.Receives.Add(receive);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReceive", new { id = receive.ReceiveId }, receive);
        }

        // DELETE: api/Receives/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceive(int id)
        {
            var receive = await _context.Receives.FindAsync(id);
            if (receive == null)
            {
                return NotFound();
            }

            _context.Receives.Remove(receive);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReceiveExists(int id)
        {
            return _context.Receives.Any(e => e.ReceiveId == id);
        }
    }
}
