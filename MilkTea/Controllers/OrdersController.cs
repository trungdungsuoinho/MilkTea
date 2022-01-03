using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MilkTea.Entities;
using ZaloPayDemo.ZaloPay;
using ZaloPayDemo.ZaloPay.Models;

namespace MilkTea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MilkTeaContext _context;
        private readonly IConfiguration Configuration;

        public OrdersController(MilkTeaContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: api/Orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            var orders = _context.Orders.ToList();
            foreach(var order in orders)
            {
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

        // GET: api/Orders/{orderId}
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order =  _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
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

        // POST: api/Orders
        [HttpPost]
        public ActionResult<Order> PostOrder(Order order)
        {
            var cart = _context.Carts.Find(order.UserId);
            if (cart == null)
            {
                return BadRequest();
            }
            _context.Entry(cart).Collection(c => c.DishCarts).Load();
            if (cart.DishCarts.Count == 0)
            {
                return BadRequest();
            }

            //Lưu Receive trước để tạo key cho Receive
            _context.Receives.Add(order.Receive);
            _context.SaveChanges();

            //Lưu Order để tạo key cho Order
            _context.Orders.Add(order);
            order.OrderDate = DateTime.Now;
            _context.SaveChanges();

            //Chép danh sách Dish từ Cart vào Order
            foreach (var dish in cart.DishCarts)
            {
                var dishOrder = new DishOrder();
                dishOrder.OrderId = order.OrderId;
                dishOrder.ProductId = dish.ProductId;
                dishOrder.Quantily = dish.Quantily;
                dishOrder.SizeName = dish.SizeName;
                dishOrder.Ice = dish.Ice;
                dishOrder.Sugar = dish.Sugar;
                dishOrder.ToppingId = dish.ToppingId;
                dishOrder.DishPrice = dish.DishPrice;
                _context.DishOrders.Add(dishOrder);
            }
            order.TotolPrice = cart.TotolPrice + order.ShipPrice;

            //Làm sạch giỏ hàng
            if (cart.DishCarts.Count != 0)
            {
                foreach (var dish in cart.DishCarts)
                {
                    _context.DishCarts.Remove(dish);
                }
                cart.TotolPrice = 0;
                _context.SaveChanges();
            }
            _context.SaveChanges();

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
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // POST: api/Orders/Online
        [HttpPost("Online")]
        public async Task<ActionResult> PostOrderOnline(Order order)
        {
            var cart = _context.Carts.Find(order.UserId);
            if (cart == null)
            {
                return BadRequest();
            }
            _context.Entry(cart).Collection(c => c.DishCarts).Load();
            if (cart.DishCarts.Count == 0)
            {
                return BadRequest();
            }

            //Lưu Receive trước để tạo key cho Receive
            _context.Receives.Add(order.Receive);
            _context.SaveChanges();

            //Lưu Order để tạo key cho Order
            _context.Orders.Add(order);
            order.OrderDate = DateTime.Now;
            _context.SaveChanges();

            //Chép danh sách Dish từ Cart vào Order
            foreach (var dish in cart.DishCarts)
            {
                var dishOrder = new DishOrder();
                dishOrder.OrderId = order.OrderId;
                dishOrder.ProductId = dish.ProductId;
                dishOrder.Quantily = dish.Quantily;
                dishOrder.SizeName = dish.SizeName;
                dishOrder.Ice = dish.Ice;
                dishOrder.Sugar = dish.Sugar;
                dishOrder.ToppingId = dish.ToppingId;
                dishOrder.DishPrice = dish.DishPrice;
                _context.DishOrders.Add(dishOrder);
            }
            order.TotolPrice = cart.TotolPrice + order.ShipPrice;

            var payOrderData = new OrderData(Configuration, order.UserId.ToString(), order.TotolPrice);
            var payOrder = await ZaloPayHelper.CreateOrder(Configuration, payOrderData);

            if (Convert.ToInt32(payOrder["returncode"]) == 1)
            {
                _context.Transactions.Add(new Transaction
                {
                    TransactionId = order.OrderId,
                    Apptransid = payOrderData.Apptransid,
                    Timestamp = payOrderData.Apptime,
                    Status = 0
                });

                //Làm sạch giỏ hàng
                if (cart.DishCarts.Count != 0)
                {
                    foreach (var dish in cart.DishCarts)
                    {
                        _context.DishCarts.Remove(dish);
                    }
                    cart.TotolPrice = 0;
                }
                _context.SaveChanges();
            
                var orderurl = payOrder["orderurl"].ToString();
                var QRCodeBase64Image = QRCodeHelper.CreateQRCodeBase64Image(orderurl);
                var apptransid = payOrderData.Apptransid;
                return Ok(new { orderurl, QRCodeBase64Image, apptransid });
            }
            else
            {
                _context.Orders.Remove(order);
                _context.Receives.Remove(order.Receive);
                _context.SaveChanges();
                return BadRequest();
            }
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
