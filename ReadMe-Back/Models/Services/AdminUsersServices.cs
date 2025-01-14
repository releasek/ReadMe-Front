using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Models.Services
{
    public class AdminUsersServices
    {
        private readonly AdminUsersEFRepo _repo;

        public AdminUsersServices(AdminUsersEFRepo repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<AdminUserDto>> GetAllAdminUsersAsync()
        {
            return await _repo.GetAllAdminUsers();
        }

        public async Task<List<CreateRoleDto>> GetAssignedRolesAsync(int userId)
        {
            return await _repo.GetAssignedRolesAsync(userId);
        }

        public async Task<List<CreateRoleDto>> GetUnassignedRolesAsync(int userId)
        {
            return await _repo.GetUnassignedRolesAsync(userId);
        }

        public async Task UpdateRolesAsync(int userId, List<int> assignedRoleIds)
        {
            await _repo.UpdateUserRolesAsync(userId, assignedRoleIds);
        }


        //-----------------------------------------------------------------
        //建立新的使用者
        public async Task CreateUserAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("使用者名稱不能為空");
            }

            // 1. 檢查使用者是否已存在
            var existingUser = await _repo.GetUserByNameAsync(userName);
            if (existingUser != null)
            {
                throw new Exception($"使用者名稱 '{userName}' 已存在");
            }

            // 2. 建立新使用者 (存到 AdminUsers)
            var newUser = await _repo.AddUserAsync(userName, password);
           
        }

       
    }




    
}
