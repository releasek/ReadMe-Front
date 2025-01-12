using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Models.Repositories
{
    public class RoleFunctionsEFRepo
    {
        private readonly AppDbContext _context;

        public RoleFunctionsEFRepo(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<RoleFunctionDto>> GetAllRoles()
        {
            return await _context.AdminRoles
                .Select(r => new RoleFunctionDto
                {
                    Id = r.Id,
                    RoleName = r.RoleName
                })
                .ToListAsync();
        }


    }
}
