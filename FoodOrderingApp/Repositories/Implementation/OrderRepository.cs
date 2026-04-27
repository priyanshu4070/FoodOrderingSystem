using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderingApp.Data;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodOrderingApp.Repositories.Implementation;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public Order? GetById(int id)
    {
        return _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .FirstOrDefault(o => o.Id == id);
    }
    public void Update(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public List<Order> GetByUser(int userId)
    {
        return _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .ToList();
    }
}
