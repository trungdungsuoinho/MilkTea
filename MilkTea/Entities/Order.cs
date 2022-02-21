using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MilkTea.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int ReceiveId { get; set; }
        public Receive Receive { get; set; }
        public string Note { get; set; }
        public int ShipPrice { get; set; }
        public int TotolPrice { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
