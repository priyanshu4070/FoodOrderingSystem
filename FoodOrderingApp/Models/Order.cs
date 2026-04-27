using FoodOrderingApp.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // One order → many items
        public List<OrderItem> Items { get; set; } = new();
    }

}