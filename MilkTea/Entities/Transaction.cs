using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTea.Entities
{
    public class Transaction
    {
        [ForeignKey("Order")]
        public int TransactionId { get; set; }
        public Order Order { get; set; }
        public string Apptransid { get; set; }
        public string Zptransid { get; set; }
        public long Timestamp { get; set; }
        public int Status { get; set; }
        public int Channel { get; set; }
    }
}
