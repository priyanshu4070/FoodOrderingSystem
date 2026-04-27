using System;
using System.Collections.Generic;
using System.Text;

using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetById(int id);
        List<Order> GetByUserId(int userId);
        List<User> GetAll();
    }
}
