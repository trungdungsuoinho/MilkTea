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
    public class OrdersController : ControllerBase
    {
        private readonly MilkTeaContext _context;

        public OrdersController(MilkTeaContext context)
        {
            _context = context;
        }

        //DONE
        // GET: api/Orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            var orders = _context.Orders.ToList();
            foreach(var order in orders)
            {
                _context.Entry(order).Reference(p => p.User).Load();
                _context.Entry(order).Reference(p => p.Receive).Load();
                _context.Entry(order).Collection(o => o.DishOrders).Load();
                foreach (var dish in order.DishOrders)
                {
                    _context.Entry(dish).Reference(p => p.Product).Load();
                    _context.Entry(dish).Reference(p => p.Size).Load();
                    if (dish.ToppingId != null)
                    {
                        _context.Entry(dish).Reference(p => p.Topping).Load();
                    }
                }
            }
            return orders;
        }

        //DONE
        // GET: api/Orders/{orderId}
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order =  _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            _context.Entry(order).Reference(p => p.User).Load();
            _context.Entry(order).Reference(p => p.Receive).Load();
            _context.Entry(order).Collection(o => o.DishOrders).Load();
            foreach (var dish in order.DishOrders)
            {
                _context.Entry(dish).Reference(p => p.Product).Load();
                _context.Entry(dish).Reference(p => p.Size).Load();
                if (dish.ToppingId != null)
                {
                    _context.Entry(dish).Reference(p => p.Topping).Load();
                }
            }
            return order;
        }

        // PUT: api/Orders/{orderId}
        [HttpPut("{id}")]
        public IActionResult PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            _context.Entry(order).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        //DONE
        // POST: api/Orders
        [HttpPost]
        public ActionResult<Order> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            order.OrderDate = DateTime.Now;
            _context.Entry(order).Reference(p => p.Receive).Load();
            foreach (var dish in order.DishOrders)
            {
                _context.Entry(dish).Reference(p => p.Product).Load();
                _context.Entry(dish).Reference(p => p.Size).Load();
                if (dish.ToppingId != null)
                {
                    _context.Entry(dish).Reference(p => p.Topping).Load();
                    dish.DishPrice = (dish.Product.Price + dish.Topping.Price) * dish.Quantily;
                }
                else
                {
                    dish.DishPrice = dish.Product.Price * dish.Quantily;
                }
            }
            order.TotolPrice = (from d in order.DishOrders select d.DishPrice).Sum() + order.ShipPrice;
            _context.SaveChanges();
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
