using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.DTOs.Order
{
    public class PlaceOrderDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
