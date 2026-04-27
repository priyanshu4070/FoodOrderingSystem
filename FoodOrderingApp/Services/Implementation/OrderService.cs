using FoodOrderingApp.DTOs.Order;
using FoodOrderingApp.Models;
using FoodOrderingApp.Models.Enum;
using FoodOrderingApp.Repositories.Interfaces;
using FoodOrderingApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IMenuItemRepository _menuRepo;

    public OrderService(IOrderRepository orderRepo, IMenuItemRepository menuRepo)
    {
        _orderRepo = orderRepo;
        _menuRepo = menuRepo;
    }

    // 🔒 NEW: Get single order with ownership validation
    public Order GetOrderById(int orderId, int currentUserId)
    {
        var order = _orderRepo.GetById(orderId);

        if (order == null)
            throw new Exception("Order not found");

        // 🔥 CORE SECURITY RULE
        if (order.UserId != currentUserId)
            throw new UnauthorizedAccessException("You cannot view this order");

        return order;
    }

    public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var order = _orderRepo.GetById(orderId);

        if (order == null)
            throw new Exception("Order not found");

        // 🚨 VALIDATION LOGIC (STATE MACHINE)
        switch (order.Status)
        {
            case OrderStatus.Pending:
                if (newStatus != OrderStatus.Confirmed && newStatus != OrderStatus.Cancelled)
                    throw new InvalidOperationException("Invalid status transition");
                break;

            case OrderStatus.Confirmed:
                if (newStatus != OrderStatus.Delivered && newStatus != OrderStatus.Cancelled)
                    throw new InvalidOperationException("Invalid status transition");
                break;

            case OrderStatus.Delivered:
            case OrderStatus.Cancelled:
                throw new InvalidOperationException("Order is already completed");
        }

        order.Status = newStatus;

        _orderRepo.Update(order);
    }

    public void PlaceOrder(int userId, List<(int menuItemId, int quantity)> items)
    {
        if (items == null || items.Count == 0)
            throw new Exception("Order cannot be empty");

        var orderItems = new List<OrderItem>();
        decimal totalPrice = 0;

        foreach (var (menuItemId, quantity) in items)
        {
            var menuItem = _menuRepo.GetById(menuItemId);

            if (menuItem == null)
                throw new Exception("Menu item not found");

            if (!menuItem.IsAvailable)
                throw new Exception($"Item '{menuItem.Name}' is not available");

            if (quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            decimal itemTotal = menuItem.Price * quantity;
            totalPrice += itemTotal;

            orderItems.Add(new OrderItem
            {
                MenuItemId = menuItemId,
                Quantity = quantity,
                PriceAtTime = menuItem.Price // 🔒 lock price at order time
            });
        }

        var order = new Order
        {
            UserId = userId,
            Items = orderItems,
            TotalPrice = totalPrice,
            Status = OrderStatus.Pending
        };

        _orderRepo.Add(order);
    }

    // 🔒 Already correct: only returns user's orders
    public List<OrderDto> GetUserOrders(int userId)
    {
        // Step 1: Get data from repository (Entity)
        var orders = _orderRepo.GetByUser(userId);

        // Step 2: Convert Entity → DTO
        var result = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            TotalPrice = o.TotalPrice,
            Status = o.Status.ToString(),

            Items = o.Items.Select(i => new OrderResponseItemDto
            { 
                MenuItemName = i.MenuItem != null ? i.MenuItem.Name : "",
                Quantity = i.Quantity,
                PriceAtTime = i.PriceAtTime
            }).ToList()
        }).ToList();

        return result;
    }
}