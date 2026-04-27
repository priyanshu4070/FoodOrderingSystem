using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.DTOs.Restaurant
{
    public class CreateRestaurantDto
    {
        public string Name { get; set; }
        public int OwnerId { get; set; }
    }
}
