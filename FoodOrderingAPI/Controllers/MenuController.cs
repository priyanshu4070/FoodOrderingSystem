using FoodOrderingApp.DTOs.Menu;
using FoodOrderingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuItemService _service;

    public MenuController(IMenuItemService service)
    {
        _service = service;
    }

    // 🔒 Protected
    [Authorize]
    [HttpPut("availability")]
    public IActionResult SetAvailability(int itemId, int restaurantId, bool isAvailable)
    {
        try
        {
            _service.SetAvailability(itemId, restaurantId, isAvailable);
            return Ok(new { message = "Availability updated" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // 🔒 Protected
    [Authorize]
    [HttpPost]
    public IActionResult Add([FromBody] CreateMenuItemDto dto)
    {
        try
        {
            _service.AddMenuItem(dto.RestaurantId, dto.Name, dto.Price);
            return Ok("Menu item added");
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // 🔒 Protected
    [Authorize]
    [HttpPut("update")]
    public IActionResult Update(int itemId, int restaurantId, string name, decimal price)
    {
        try
        {
            _service.UpdateMenuItem(itemId, restaurantId, name, price);
            return Ok(new { message = "Menu item updated" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // 🔒 Protected
    [Authorize]
    [HttpDelete]
    public IActionResult Delete(int itemId, int restaurantId)
    {
        try
        {
            _service.DeleteMenuItem(itemId, restaurantId);
            return Ok(new { message = "Menu item deleted" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // 🌐 Public
    [AllowAnonymous]
    [HttpGet("{restaurantId}")]
    public IActionResult Get(int restaurantId)
    {
        return Ok(_service.GetMenu(restaurantId));
    }
}
