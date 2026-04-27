using FoodOrderingApp.DTOs.User;
using FoodOrderingApp.DTOs.Menu;
using FoodOrderingApp.DTOs.Order;
using FoodOrderingApp.DTOs.Restaurant;
using FoodOrderingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _service;

    public RestaurantController(IRestaurantService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateRestaurantDto dto)
    {
        _service.AddRestaurant(dto.Name, dto.OwnerId);
        return Ok("Restaurant added");
    }

    // GET: api/Restaurant
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetRestaurants());
    }
}
