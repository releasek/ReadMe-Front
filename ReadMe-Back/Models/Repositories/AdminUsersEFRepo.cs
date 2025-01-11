using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Models.Repositories
{
    public class AdminUsersEFRepo
    {
        private readonly AppDbContext _context;

        public AdminUsersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AdminUserDto>> GetAllAdminUsers()
        {
            return await _context.AdminUsers
                .Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    Name = u.UserName,
                    GroupName = u.GroupName,
                    PermissionName = u.PermissionName
                })
                .ToListAsync();
        }


    }
}
