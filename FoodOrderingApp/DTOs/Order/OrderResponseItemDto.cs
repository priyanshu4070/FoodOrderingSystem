using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.DTOs.Order
{
    public class OrderResponseItemDto
    {
        public string MenuItemName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
    }
}
