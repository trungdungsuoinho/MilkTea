using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static MilkTea.Entities.Enums;

namespace MilkTea.Entities
{
    public abstract class Dish
    {
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantily { get; set; }
        [ForeignKey("Size")]
        public string SizeName { get; set; }
        public Size Size { get; set; }
        public Ice Ice { get; set; }
        public Sugar Sugar { get; set; }
        public int? ToppingId { get; set; }
        public Topping Topping { get; set; }
        public int DishPrice { get; set; }
    }

    public class DishOrder : Dish
    {
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
    }

    public class DishCart : Dish
    {
        public int CartId { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }
    }
}
