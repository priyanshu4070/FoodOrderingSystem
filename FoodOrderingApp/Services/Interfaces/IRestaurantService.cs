using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Models;
using FoodOrderingApp.DTOs.Restaurant;

namespace FoodOrderingApp.Services.Interfaces
{
    public interface IRestaurantService
    {
        void AddRestaurant(string name, int ownerId);
        public List<RestaurantDto> GetRestaurants();
    }
}
