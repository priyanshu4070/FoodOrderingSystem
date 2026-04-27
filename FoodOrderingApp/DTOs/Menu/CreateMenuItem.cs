using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.DTOs.Menu
{
    public class CreateMenuItemDto
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
