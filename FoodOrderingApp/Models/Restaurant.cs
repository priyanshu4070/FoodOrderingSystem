using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public int OwnerId { get; set; }   // NEW

        public User? Owner { get; set; }   // optional navigation

        // One restaurant → many menu items
        public List<MenuItem> MenuItems { get; set; } = new();
    }
}
