using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        void Add(MenuItem item);
        void Update(MenuItem item);
        void Delete(int id); // 🔥 ADDED

        List<MenuItem> GetByRestaurant(int restaurantId);
        MenuItem? GetById(int id);
    }
}
