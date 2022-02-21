using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Entities;

namespace MilkTea.Controllers
{
    //[Authorize]
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
            return await _context.DishCarts.ToListAsync();
        }

        // GET: api/DishCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DishCart>> GetDishCart(int id)
        {
            var dishCart = await _context.DishCarts.FindAsync(id);

            if (dishCart == null)
            {
                return NotFound();
            }

            return dishCart;
        }

        //Edit Dish in Cart
        // PUT: api/DishCarts/{dishId}
        [HttpPut("{id}")]
        public ActionResult<Cart> PutDishCart(int id, DishCart dishCart)
        {
            if (id != dishCart.DishId || !_context.DishCarts.Any(e => e.DishId == id && e.CartId == dishCart.CartId))
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

        //Add new Dish into Cart
        // POST: api/DishCarts
        [HttpPost]
        public ActionResult<Cart> PostDishCart(DishCart dishCart)
        {
            //Create a new Cart if it doesn't already exist 
            var cart = _context.Carts.Find(dishCart.CartId);
            if (cart == null)
            {
                cart = new Cart { CartId = dishCart.CartId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            //Add Dish and calculate DishPrice
            _context.DishCarts.Add(dishCart);
            CalculateDishCart(dishCart);
            _context.SaveChanges();

            _context.Entry(cart).Collection(c => c.DishCarts).Load();
            if (cart.DishCarts.Count != 0)
            {
                CalculateCart(cart);
                _context.SaveChanges();
                foreach (var dish in cart.DishCarts)
                {
                    _context.Entry(dish).Reference(d => d.Product).Load();
                }
            }
            return CreatedAtAction("GetDishCart", new { id = cart.CartId }, cart);
        }

        //Remove Dish from Cart
        // DELETE: api/DishCarts/{dishId}
        [HttpDelete("{id}")]
        public ActionResult<Cart> DeleteDishCart(int id)
        {
            var dishCart = _context.DishCarts.Find(id);
            if (dishCart == null)
            {
                return NotFound();
            }
            _context.DishCarts.Remove(dishCart);
            var cart = _context.Carts.Find(dishCart.CartId);
            _context.SaveChanges();

            _context.Entry(cart).Collection(c => c.DishCarts).Load();
            if (cart.DishCarts.Count != 0)
            {
                CalculateCart(cart);
                _context.SaveChanges();
                foreach (var dish in cart.DishCarts)
                {
                    if (dish.ToppingId != null)
                    {
                        _context.Entry(dish).Reference(d => d.Topping).Load();
                        dish.DishPrice = dish.Topping.Price;
                    }
                    _context.Entry(dish).Reference(d => d.Product).Load();
                    _context.Entry(dish).Reference(d => d.Size).Load();
                }
            }
            return CreatedAtAction("GetDishCart", new { id = cart.CartId }, cart);
        }

        private bool DishCartExists(int id)
        {
            return _context.DishCarts.Any(e => e.DishId == id);
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
