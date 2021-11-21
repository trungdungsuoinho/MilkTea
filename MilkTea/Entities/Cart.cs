using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MilkTea.Entities
{
    public class Cart
    {
        [ForeignKey("User")]
        public int CartId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int TotolPrice { get; set; }
        public ICollection<DishCart> DishCarts { get; set; }
    }
}
