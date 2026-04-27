using System;
using System.Collections.Generic;


namespace FoodOrderingApp.Models
{
   public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";   // NEW

        public string Role { get; set; } = "User";   // NEW (User/Admin)
        public List<Order> Orders { get; set; } = new();
    }
}
