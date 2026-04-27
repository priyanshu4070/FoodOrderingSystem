using FoodOrderingApp.DTOs.Menu;
using FoodOrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Services.Interfaces
{
    public interface IMenuItemService
    {
        void AddMenuItem(int restaurantId, string name, decimal price);

        public List<MenuItemDto> GetMenu(int restaurantId);

        // 🔒 UPDATED (ownership enforced)
        void SetAvailability(int itemId, int restaurantId, bool isAvailable);

        // 🔥 NEW (needed for full control)
        void UpdateMenuItem(int itemId, int restaurantId, string name, decimal price);

        void DeleteMenuItem(int itemId, int restaurantId);
    }
}
