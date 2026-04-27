using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.Interfaces;

namespace FoodOrderingApp.Repositories.Implementation;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly AppDbContext _context;

    public RestaurantRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Restaurant restaurant)
    {
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();
    }

    public List<Restaurant> GetAll()
    {
        return _context.Restaurants.ToList();
    }

    public Restaurant? GetById(int id)
    {
        return _context.Restaurants.Find(id);
    }
}