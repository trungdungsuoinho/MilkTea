using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Entities;

namespace MilkTea.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly MilkTeaContext _context;

        public CartsController(MilkTeaContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public ActionResult<IEnumerable<Cart>> GetCarts()
        {
            var carts = _context.Carts.ToList();
            foreach (var cart in carts)
            {
                _context.Entry(cart).Collection(c => c.DishCarts).Load();
                if (cart.DishCarts != null)
                {
                    foreach (var dish in cart.DishCarts)
                    {
                        _context.Entry(dish).Reference(c => c.Product).Load();
                        _context.Entry(dish).Reference(c => c.Size).Load();
                        _context.Entry(dish).Reference(c => c.Topping).Load();
                    }
                }
            }
            return carts;
        }

        // GET: api/Carts/{userId}
        [HttpGet("{id}")]
        public ActionResult<Cart> GetCart(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                _context.Entry(cart).Collection(c => c.DishCarts).Load();
                if (cart.DishCarts != null)
                {
                    foreach (var dish in cart.DishCarts)
                    {
                        _context.Entry(dish).Reference(c => c.Product).Load();
                        _context.Entry(dish).Reference(c => c.Size).Load();
                        _context.Entry(dish).Reference(c => c.Topping).Load();
                    }
                }
                return cart;
            }
            return NotFound("Cart does not exist");
        }


        // PUT: api/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }
            var cartExist = _context.Carts.Find(cart.CartId);
            _context.Entry(cartExist).State = EntityState.Modified;
            var dish = cart.DishCarts.Where(d => d.DishId == cart.DishCarts.First().DishId).First();
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        //Thêm món mới vào giỏ hàng
        // POST: api/Carts
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            //Neu them mot mon moi vao gio hang
            if (cart.DishCarts.Count > 0)
            {
                var dish = cart.DishCarts.First();
                var cartExist = _context.Carts.Find(cart.CartId);
                //Neu gio hang da ton tai
                if (cartExist != null)
                {
                    cartExist.DishCarts.Add(dish);
                    //_context.Entry(cartExist).Property(c => c.DishCarts).CurrentValue.Add(dish);
                    CalculateDishCart(dish);
                    CalculateCart(cartExist);
                    cart = cartExist;
                }
                //Neu gio hang chua ton tai
                else
                {
                    _context.Carts.Add(cart);
                    CalculateDishCart(dish);
                    CalculateCart(cart);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (CartExists(cart.CartId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }

        private void CalculateDishCart(DishCart dishCart)
        {
            if (dishCart.ToppingId != null)
            {
                _context.Entry(dishCart).Reference(d => d.Topping).Load();
                dishCart.DishPrice = dishCart.Topping.Price;
            }
            _context.Entry(dishCart).Reference(d => d.Product).Load();
            _context.Entry(dishCart).Reference(d => d.Size).Load();
            dishCart.DishPrice = (dishCart.DishPrice + dishCart.Product.Price) * dishCart.Quantily;
        }

        private void CalculateCart(Cart cart)
        {
            if (cart.DishCarts.Count() > 0)
            {
                _context.Entry(cart).Collection(c => c.DishCarts).Load();
                cart.TotolPrice = (from d in cart.DishCarts
                                   select d.DishPrice).Sum();
            }
            else
            {
                cart.TotolPrice = 0;
            }
        }
    }
}
