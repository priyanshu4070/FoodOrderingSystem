using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
      
        //foreign key to restaurant
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<OrderItem> Items { get; set; } = new();
    }
}
