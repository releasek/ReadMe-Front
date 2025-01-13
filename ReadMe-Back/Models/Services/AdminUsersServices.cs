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

        public async Task<List<RoleDto>> GetAssignedRolesAsync(int userId)
        {
            return await _repo.GetAssignedRolesAsync(userId);
        }

        public async Task<List<RoleDto>> GetUnassignedRolesAsync(int userId)
        {
            return await _repo.GetUnassignedRolesAsync(userId);
        }

        public async Task UpdateRolesAsync(int userId, List<int> assignedRoleIds)
        {
            await _repo.UpdateUserRolesAsync(userId, assignedRoleIds);
        }
    }
}
