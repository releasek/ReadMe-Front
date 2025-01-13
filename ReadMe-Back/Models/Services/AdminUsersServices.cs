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
        public async Task CreateUserAsync(string userName, List<int> assignedRoleIds)
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
            var newUser = await _repo.AddUserAsync(userName);

            // 3. 為該使用者分配角色 (寫入 AdminUserRoleRels)
            if (assignedRoleIds != null && assignedRoleIds.Any())
            {
                foreach (var roleId in assignedRoleIds)
                {
                    await _repo.AssignRoleToUserAsync(newUser.Id, roleId);
                }
            }
        }

        public async Task<List<UserVm>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return users.Select(u => new UserVm
            {
                Id = u.Id,
                UserName = u.UserName
                // 如需帶出角色，可在 Repository 做 Include 或另查 UserRoleRels
            }).ToList();
        }
    }




    
}
