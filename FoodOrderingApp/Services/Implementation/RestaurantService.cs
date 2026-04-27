using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.Interfaces;
using FoodOrderingApp.Services.Interfaces;
using FoodOrderingApp.DTOs.Restaurant;

namespace FoodOrderingApp.Services.Implementation;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _repo;

    public RestaurantService(IRestaurantRepository repo)
    {
        _repo = repo;
    }

    public void AddRestaurant(string name, int ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Invalid name");

        _repo.Add(new Restaurant
        {
            Name = name,
            OwnerId = ownerId
        });
    
    }

    public List<RestaurantDto> GetRestaurants()
    {
        return _repo.GetAll()
            .Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
    }
}