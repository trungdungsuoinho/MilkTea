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
    public class DishCartsController : ControllerBase
    {
        private readonly MilkTeaContext _context;

        public DishCartsController(MilkTeaContext context)
        {
            _context = context;
        }

        // GET: api/DishCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishCart>>> GetDishCart()
        {
            return await _context.DishCart.ToListAsync();
        }

        // GET: api/DishCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DishCart>> GetDishCart(int id)
        {
            var dishCart = await _context.DishCart.FindAsync(id);

            if (dishCart == null)
            {
                return NotFound();
            }

            return dishCart;
        }

        //Sua mon trong gio hang
        // PUT: api/DishCarts/{dishId}
        [HttpPut("{id}")]
        public ActionResult<Cart> PutDishCart(int id, DishCart dishCart)
        {
            if (id != dishCart.DishId || !_context.DishCart.Any(e => e.DishId == id && e.CartId == dishCart.CartId))
            {
                return BadRequest();
            }
            var cart = _context.Carts.Find(dishCart.CartId);
            if (cart != null)
            {
                _context.Entry(dishCart).State = EntityState.Modified;
                CalculateDishCart(dishCart);
                CalculateCart(cart);
                _context.SaveChanges();
                _context.Entry(cart).Collection(c => c.DishCarts);
                return CreatedAtAction("GetDishCart", new { id = cart.CartId }, cart);
            }
            return BadRequest();
        }

        //Them mon moi vao gio hang
        // POST: api/DishCarts
        [HttpPost]
        public ActionResult<Cart> PostDishCart(DishCart dishCart)
        {
            var cart = _context.Carts.Find(dishCart.CartId);
            if (cart == null)
            {
                cart = new Cart { CartId = dishCart.CartId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            _context.DishCart.Add(dishCart);
            CalculateDishCart(dishCart);
            _context.SaveChanges();
            _context.Entry(cart).Collection(c => c.DishCarts);
            foreach (var dish in cart.DishCarts)
            {
                _context.Entry(dish).Reference(d => d.Product);
            }
            CalculateCart(cart);
            _context.SaveChanges();
            return CreatedAtAction("GetDishCart", new { id = cart.CartId }, cart);
        }

        //Xoa mon khoi gio hang
        // DELETE: api/DishCarts/{dishId}
        [HttpDelete("{id}")]
        public ActionResult<Cart> DeleteDishCart(int id)
        {
            var dishCart = _context.DishCart.Find(id);
            if (dishCart == null)
            {
                return NotFound();
            }
            _context.DishCart.Remove(dishCart);
            var cart = _context.Carts.Find(dishCart.CartId);
            _context.SaveChanges();
            _context.Entry(cart).Collection(c => c.DishCarts).Load();
            CalculateCart(cart);
            _context.SaveChanges();
            return CreatedAtAction("GetDishCart", new { id = cart.CartId }, cart);
        }

        private bool DishCartExists(int id)
        {
            return _context.DishCart.Any(e => e.DishId == id);
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
