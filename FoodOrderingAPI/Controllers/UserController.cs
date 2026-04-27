using FoodOrderingApp.DTOs.User;
using FoodOrderingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodOrderingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IConfiguration _config;

    public UserController(IUserService service, IConfiguration config)
    {
        _service = service;
        _config = config;
    }

    // ================= REGISTER =================
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] CreateUserDto dto)
    {
        _service.Register(dto.Name, dto.Email, dto.Password);
        return Ok("User registered");
    }

    // ================= LOGIN =================
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _service.ValidateUser(dto.Email, dto.Password);

        if (user == null)
            return Unauthorized("Invalid email or password");

        var token = GenerateJwtToken(user.Id);

        return Ok(new { token });
    }

    // ================= GET USERS =================
    [Authorize] // 🔒 protected
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAllUsers());
    }

    // ================= JWT GENERATOR =================
    private string GenerateJwtToken(int userId)
    {
        var jwtSettings = _config.GetSection("Jwt");

        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}