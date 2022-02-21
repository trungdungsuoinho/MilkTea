using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MilkTea.Entities
{
    public class Receive
    {
        public int ReceiveId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public string ReceiveName { get; set; }
        public string ReceivePhone { get; set; }
        public string ReceiveAddress { get; set; }
        public string ReceiveNote { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
