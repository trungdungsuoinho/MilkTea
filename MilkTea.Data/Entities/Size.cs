using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MilkTea.Entities
{
    public class Size
    {
        [Key]
        [MaxLength(1)]
        public string SizeName { get; set; }
        public int Price { get; set; }
        [JsonIgnore]
        public ICollection<Dish> Dishs { get; set; }
    }
}
