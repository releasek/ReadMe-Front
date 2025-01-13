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

        public async Task<List<AdminRole>> GetAllRolesAsync()
        {
            // 從資料庫獲取角色資料
            return await _context.AdminRoles.ToListAsync();
        }

        // 獲取角色已分配的功能
        public async Task<List<RoleFunctionDto>> GetAssignedFunctionsAsync(int roleId)
        {
            return await _context.AdminRoleFunctionRels
                .Where(rel => rel.RoleId == roleId)
                .Select(rel => new RoleFunctionDto
                {
                    Id = rel.FunctionId,
                    FunctionName = rel.Function.FunctionName
                })
                .ToListAsync();
        }

        // 獲取角色未分配的功能
        public async Task<List<RoleFunctionDto>> GetUnassignedFunctionsAsync(int roleId)
        {
            var assignedFunctionIds = await _context.AdminRoleFunctionRels
                .Where(rel => rel.RoleId == roleId)
                .Select(rel => rel.FunctionId)
                .ToListAsync();

            return await _context.AdminRoleFunctions
                .Where(func => !assignedFunctionIds.Contains(func.Id))
                .Select(func => new RoleFunctionDto
                {
                    Id = func.Id,
                    FunctionName = func.FunctionName
                })
                .ToListAsync();
        }

        // 更新角色功能
        public async Task UpdateRoleFunctionsAsync(int roleId, List<int> assignedFunctionIds)
        {
            Console.WriteLine($"清空現有功能關聯: RoleId = {roleId}");
            var existingRels = _context.AdminRoleFunctionRels.Where(rf => rf.RoleId == roleId);
            _context.AdminRoleFunctionRels.RemoveRange(existingRels);

            foreach (var functionId in assignedFunctionIds)
            {
                Console.WriteLine($"新增功能關聯: RoleId = {roleId}, FunctionId = {functionId}");
                _context.AdminRoleFunctionRels.Add(new AdminRoleFunctionRel
                {
                    RoleId = roleId,
                    FunctionId = functionId
                });
            }

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("資料庫更新成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"資料庫更新失敗: {ex.Message}");
                throw;
            }
        }

        //新增角色
        public async Task<List<AdminRoleFunction>> GetAllFunctionsAsync()
        {
            return await _context.AdminRoleFunctions.ToListAsync();
        }

        // 獲取角色是否已存在
        public async Task<AdminRole> GetRoleByNameAsync(string roleName)
        {
            return await _context.AdminRoles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        // 添加新角色
        public async Task<AdminRole> AddRoleAsync(string roleName)
        {
            var role = new AdminRole { RoleName = roleName };
            await _context.AdminRoles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        // 為角色添加功能
        public async Task AddRoleFunctionAsync(int roleId, int functionId)
        {
            var rel = new AdminRoleFunctionRel
            {
                RoleId = roleId,
                FunctionId = functionId
            };
            await _context.AdminRoleFunctionRels.AddAsync(rel);
            await _context.SaveChangesAsync();
        }



    }


}

