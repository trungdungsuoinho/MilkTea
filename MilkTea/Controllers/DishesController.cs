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
    public class DishesController : ControllerBase
    {
        private readonly MilkTeaContext _context;

        public DishesController(MilkTeaContext context)
        {
            _context = context;
        }

        //GET: api/Dishes
       [HttpGet]
        public ActionResult<IEnumerable<Dish>> GetDishes()
        {
            var dishes = _context.Dishes.ToList();
            foreach (var dish in dishes)
            {
                _context.Entry(dish).Reference(p => p.Product).Load();
                _context.Entry(dish).Reference(p => p.Size).Load();
                _context.Entry(dish).Reference(p => p.Topping).Load();
                dish.DishPrice = (dish.Product.Price + dish.Topping.Price) * dish.Quantily;
            }
            return dishes;
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public ActionResult<Dish> GetDish(int id)
        {
            var dish = _context.Dishes.Find(id);
            _context.Entry(dish).Reference(p => p.Product).Load();
            _context.Entry(dish).Reference(p => p.Size).Load();
            _context.Entry(dish).Reference(p => p.Topping).Load();
            dish.DishPrice = (dish.Product.Price + dish.Topping.Price) * dish.Quantily;
            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.DishId)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
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

        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Dish> PostDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            _context.SaveChanges();

            return CreatedAtAction("GetDish", new { id = dish.DishId }, dish);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
