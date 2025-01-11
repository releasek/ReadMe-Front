using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Models.Repositories
{
    public class AdminUsersEFRepo
    {
        private readonly AppDbContext _context;

        public AdminUsersEFRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminUserDto>> GetAllAdminUsers()
        {
                return await _context.AdminUsers
                    .Select(u => new AdminUserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName
                    })
                    .ToListAsync();
        }
    }
}
