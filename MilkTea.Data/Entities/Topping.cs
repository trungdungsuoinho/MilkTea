using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MilkTea.Entities
{
    public class Topping
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; }
        public int Price { get; set; }
        [JsonIgnore]
        public ICollection<Dish> Dishs { get; set; }
    }
}
