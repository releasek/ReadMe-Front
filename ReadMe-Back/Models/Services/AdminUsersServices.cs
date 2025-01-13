using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.Repositories;

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
         public async Task<List<AdminUserDto>> GetAllRolesAsync()
        {
            var roles = await _repo.GetAllRolesAsync();
            return roles.Select(r => new AdminUserDto
            {
                Id = r.Id,
                RoleName = r.RoleName
            }).ToList();
        }
 

        public async Task CreateFunctionAsync(string functionName, List<int> assignedRoleIds)
        {
            if (string.IsNullOrWhiteSpace(functionName))
            {
                throw new ArgumentException("功能名稱不能為空");
            }

            // 檢查功能是否已存在
            var existingFunction = await _repo.GetFunctionByNameAsync(functionName);
            if (existingFunction != null)
            {
                throw new Exception($"功能名稱 '{functionName}' 已存在");
            }

            // 創建新功能
            var newFunction = await _repo.AddFunctionAsync(functionName);

            // 為新功能分配角色
            if (assignedRoleIds != null && assignedRoleIds.Any())
            {
                foreach (var roleId in assignedRoleIds)
                {
                    await _repo.AssignRoleToFunctionAsync(newFunction.Id, roleId);
                }
            }
        }


    }
}
