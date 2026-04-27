using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.Interfaces;
using System.Linq;

namespace FoodOrderingApp.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }
    public List<Order> GetByUserId(int userId)
    {
        return _context.Orders
            .Where(o => o.UserId == userId)
            .ToList();
    }
}
