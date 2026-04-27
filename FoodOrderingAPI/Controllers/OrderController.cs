using FoodOrderingApp.DTOs.Order;
using FoodOrderingApp.Models.Enum;
using FoodOrderingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodOrderingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // 🔒 PROTECT ALL ORDER APIs
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    // 🔑 Extract userId from JWT
    private int GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            throw new Exception("User not authenticated");

        return int.Parse(userId);
    }

    // ================= UPDATE STATUS =================
    [HttpPut("status")]
    public IActionResult UpdateStatus(int orderId, OrderStatus status)
    {
        try
        {
            _service.UpdateOrderStatus(orderId, status);
            return Ok(new { message = "Order status updated" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // ================= PLACE ORDER =================
    [HttpPost("place")]
    public IActionResult Place([FromBody] PlaceOrderDto dto)
    {
        try
        {
            var userId = GetUserId(); // 🔥 from JWT

            var mapped = dto.Items.Select(i => (i.MenuItemId, i.Quantity)).ToList();

            _service.PlaceOrder(userId, mapped);

            return Ok("Order placed");
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // ================= GET MY ORDERS =================
    [HttpGet("user")]
    public IActionResult GetUserOrders()
    {
        try
        {
            var userId = GetUserId(); // 🔥 from JWT

            return Ok(_service.GetUserOrders(userId));
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // ================= GET ORDER BY ID =================
    [HttpGet("{orderId}")]
    public IActionResult GetOrderById(int orderId)
    {
        try
        {
            var userId = GetUserId(); // 🔥 from JWT

            var order = _service.GetOrderById(orderId, userId);

            return Ok(order);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}