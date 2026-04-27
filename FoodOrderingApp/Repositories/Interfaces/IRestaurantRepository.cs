using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        void Add(Restaurant restaurant);
        List<Restaurant> GetAll();
        Restaurant? GetById(int id);
    }
}
