using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodOrderingApp.Repositories.Implementation;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly AppDbContext _context;

    public MenuItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(MenuItem item)
    {
        _context.MenuItems.Add(item);
        _context.SaveChanges();
    }

    public void Update(MenuItem item)
    {
        _context.MenuItems.Update(item);
        _context.SaveChanges();
    }

    // 🔥 NEW: Delete method
    public void Delete(int id)
    {
        var item = _context.MenuItems.Find(id);

        if (item == null)
            throw new Exception("Menu item not found");

        _context.MenuItems.Remove(item);
        _context.SaveChanges();
    }

    public List<MenuItem> GetByRestaurant(int restaurantId)
    {
        return _context.MenuItems
            .Where(m => m.RestaurantId == restaurantId)
            .ToList();
    }

    public MenuItem? GetById(int id)
    {
        return _context.MenuItems.Find(id);
    }
}