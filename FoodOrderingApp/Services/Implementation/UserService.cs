using System;
using System.Collections.Generic;
using System.Linq;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.Interfaces;
using FoodOrderingApp.Services.Interfaces;
using FoodOrderingApp.DTOs.User;

namespace FoodOrderingApp.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    // ================= REGISTER =================

    public void Register(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Invalid user data");
        }

        var user = new User
        {
            Name = name,
            Email = email,
            Password = password   // ⭐ store password
        };

        _repo.Add(user);
    }

    // ================= LOGIN VALIDATION =================

    public User? ValidateUser(string email, string password)
    {
        return _repo.GetAll()
            .FirstOrDefault(u =>
                u.Email == email &&
                u.Password == password);
    }

    // ================= GET USERS =================

    public List<UserDto> GetAllUsers()
    {
        return _repo.GetAll()
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList();
    }
}
