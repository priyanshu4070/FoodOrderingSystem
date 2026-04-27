using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.DTOs.Menu
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
