using System;
using System.Collections.Generic;
using FoodOrderingApp.Models;
using FoodOrderingApp.DTOs.User;

namespace FoodOrderingApp.Services.Interfaces
{
    public interface IUserService
    {
        void Register(string name, string email, string password);   // ⭐ updated
        List<UserDto> GetAllUsers();

        User? ValidateUser(string email, string password);           // ⭐ NEW
    }
}
