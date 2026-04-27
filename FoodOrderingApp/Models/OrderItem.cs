using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public int MenuItemId { get; set; }

        public MenuItem? MenuItem { get; set; }
        public decimal PriceAtTime { get; set; }

        public int Quantity { get; set; }
    }
}
