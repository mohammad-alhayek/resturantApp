using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Core.Services;
using RestaurantStore.Shared.UserDto; // مهم جداً

namespace RestaurantStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // ✅ Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _userService.CreateUserAsync(
                model.Username,
                model.Email,
                model.Password,
                model.Role,
                model.City,
                model.Street,
                model.State,
                model.FullName,
                model.phonenumber
               

                
            );

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User created successfully");
        }

        // ✅ Change Password
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _userService.ChangePasswordAsync(
                model.UserId,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password changed successfully");
        }

        //  Get user by username
        [HttpGet("by-name")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            var result = await _userService.GetUserByUsernameAsync(userName);

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }

        //  Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // 1. التحقق من صحة بيانات الدخول
            var success = await _userService.Login(model.Username, model.Password);

            if (!success)
                return Unauthorized("Invalid username or password");

            // 2. جلب بيانات المستخدم كاملة
            var user = await _userService.GetUserByUsernameAsync(model.Username);

            // 3. جلب الأدوار (Roles)
            var roles = await _userService.GetUserRoles(user.Id); // نمرر الـ ID ونحصل على القائمة
            var primaryRole = roles.FirstOrDefault(); // نأخذ الدور الأول (مثلاً Admin)

            // 4. إرجاع كل البيانات المهمة للـ MVC في الـ Response
            return Ok(new
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = primaryRole,
                // Token = _tokenService.CreateToken(user) // إذا كنت تستخدم JWT
            });
        }

        //  Get all users
        [HttpGet("all-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        //  Assign Role
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            await _userService.AssignRoleAsync(model.UserId, model.Role);
            return Ok("Role assigned");
        }

        // ✅ Delete user
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);

            if (!result)
                return NotFound("User not found");

            return Ok("User deleted");
        }

        //updateuser
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserResponseDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> UserById(string id)
        {
            
            var result = await _userService.GetUserByIdAsync(id);
           
            return Ok(result);
        }
    }
}