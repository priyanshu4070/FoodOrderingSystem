using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void Update(Order order);
        Order? GetById(int id);
        List<Order> GetByUser(int userId);
        

    }
}
