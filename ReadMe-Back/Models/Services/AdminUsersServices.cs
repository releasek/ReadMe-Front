using NuGet.Protocol.Core.Types;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.Repositories;

namespace ReadMe_Back.Models.Services
{
    public class AdminUsersServices
    {
        private readonly AdminUsersEFRepo _repo;

        public AdminUsersServices(AdminUsersEFRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<AdminUserDto>> GetAllAdminUsers()
        {
            return await _repo.GetAllAdminUsers();
        }
    }
}
