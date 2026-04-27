using FoodOrderingApp.DTOs.Menu;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.Interfaces;
using FoodOrderingApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Services.Implementation;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _repo;

    public MenuItemService(IMenuItemRepository repo)
    {
        _repo = repo;
    }

    // 🔒 Add item (safe: restaurantId comes from logged-in context)
    public void AddMenuItem(int restaurantId, string name, decimal price)
    {
        if (price <= 0)
            throw new Exception("Invalid price");

        _repo.Add(new MenuItem
        {
            Name = name,
            Price = price,
            RestaurantId = restaurantId,
            IsAvailable = true
        });
    }

    // 🔒 Set availability ONLY if item belongs to that restaurant
    public void SetAvailability(int itemId, int restaurantId, bool isAvailable)
    {
        var item = _repo.GetById(itemId);

        if (item == null)
            throw new Exception("Menu item not found");

        // 🔥 CORE SECURITY RULE
        if (item.RestaurantId != restaurantId)
            throw new UnauthorizedAccessException("You cannot modify this menu item");

        item.IsAvailable = isAvailable;

        _repo.Update(item);
    }

    // 🔒 Update item (ownership check)
    public void UpdateMenuItem(int itemId, int restaurantId, string name, decimal price)
    {
        var item = _repo.GetById(itemId);

        if (item == null)
            throw new Exception("Menu item not found");

        if (item.RestaurantId != restaurantId)
            throw new UnauthorizedAccessException("You cannot update this menu item");

        if (price <= 0)
            throw new Exception("Invalid price");

        item.Name = name;
        item.Price = price;

        _repo.Update(item);
    }

    // 🔒 Delete item (ownership check)
    public void DeleteMenuItem(int itemId, int restaurantId)
    {
        var item = _repo.GetById(itemId);

        if (item == null)
            throw new Exception("Menu item not found");

        if (item.RestaurantId != restaurantId)
            throw new UnauthorizedAccessException("You cannot delete this menu item");

        _repo.Delete(itemId);
    }

    // 🔒 View only own menu
    public List<MenuItemDto> GetMenu(int restaurantId)
    {
        return _repo.GetByRestaurant(restaurantId)
            .Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                IsAvailable = m.IsAvailable
            }).ToList();
    }
}