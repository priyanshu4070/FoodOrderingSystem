using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "";
        public List<OrderResponseItemDto> Items { get; set; } = new();
    }
}
