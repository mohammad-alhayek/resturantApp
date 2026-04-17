using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantStore.Shared.UserDto
{
    public class RegisterDto
    {
        public string? Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; } = "Staff";
        public string phonenumber { get; set; }
        public string City { get; set; } = "zarqa";
        public string Street { get; set; } = "";
        public string State { get; set; } = "jabal tareq";
        public string FullName { get; set; } = "";
        
    }

    public class LoginDto
    {
        public LoginDto() { }   
        public string Username { get; set; }
        public string Password { get; set; }
    
    }
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
    public class ChangePasswordDto
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class AssignRoleDto
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber {  get; set; }
        public string FullName { get; set; }
        
    }
}
