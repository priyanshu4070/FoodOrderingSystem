using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.DTOs.User
{
    

    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
