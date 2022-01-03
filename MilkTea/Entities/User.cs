using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MilkTea.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(450)]
        public string UserName { get; set; }
        public bool VerifyPhone { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool VerifyEmail { get; set; }
        public bool Gender { get; set; }
        public Receive Receive { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
    }
}
