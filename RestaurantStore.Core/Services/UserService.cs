using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantStore.Core.Data;
using RestaurantStore.Core.Models;
using RestaurantStore.Shared.UserDto;

namespace RestaurantStore.Core.Services
{//still update user and update role
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager , AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }



        public async Task<bool> UpdateUserAsync(string id, UserResponseDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            user.PhoneNumber = dto.PhoneNumber;
            user.UserName = dto.Username;
            user.Email = dto.Email;
            user.FullName = dto.FullName;

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            // ✅ تأكد الـ Role موجود قبل ما تضيفه
            var roleExists = await _roleManager.RoleExistsAsync(dto.Role);
            if (roleExists)
            {
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return true;
        }
        public async Task<IdentityResult> CreateUserAsync(
            string username,
            string email,
            string password,
            string role = "Customer",
            string city = "zarqa",
            string Street = "",
            string state = "",
            string fullName = "",
            string phonenumber="")
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                City=city,
                Street=Street,
                State=state,
                FullName=fullName,
                PhoneNumber=phonenumber
                //Role=role
                
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(user, role);
            }

            return result;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FullName = user.FullName,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return result;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var result= await _userManager.FindByIdAsync(id);
            if(result==null)
                throw new Exception($"user {id} not found");
            return result;
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new Exception($"User {username} not found");

            return user;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            await _userManager.DeleteAsync(user);
            return true;
        }

        public async Task<bool> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            string userId,
            string currentPassword,
            string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task AssignRoleAsync(string userId, string role)
        {   
            var user = await _userManager.FindByIdAsync(userId);
          //  user.Role = role;
            //await _context.SaveChangesAsync();

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
            
            await _userManager.AddToRoleAsync(user, role);
        }
        public async Task<IList<string>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new List<string>(); // أو ارفع Exception إذا كان المستخدم غير موجود
            }

            // 2. جلب الأدوار المرتبطة بهذا المستخدم
            var roles = await _userManager.GetRolesAsync(user);

            return roles; // سيرجع قائمة مثل ["Admin", "Manager"]
        }
    }
}