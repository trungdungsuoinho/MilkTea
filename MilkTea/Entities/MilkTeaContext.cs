using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MilkTea.Entities;

namespace MilkTea.Entities
{
    public class MilkTeaContext : DbContext
    {
        public MilkTeaContext(DbContextOptions<MilkTeaContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Receive> Receives { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<DishCart> DishCarts { get; set; }
        public DbSet<DishOrder> DishOrders { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
