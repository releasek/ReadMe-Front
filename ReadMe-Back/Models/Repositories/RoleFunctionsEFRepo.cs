using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        //更新角色
        public async Task UpdateUserRolesAsync(int roleId, List<int> assignedFunctionIds)
        {
            Console.WriteLine($"開始更新用戶角色: UserId = {roleId}, AssignedRoleIds = [{string.Join(", ", assignedFunctionIds)}]");

            if (roleId <= 0 || assignedFunctionIds == null || !assignedFunctionIds.Any())
            {
                throw new ArgumentException("無效的參數");
            }

            try
            {
                // 確認用戶是否存在
                var role = await _context.AdminRoles.FirstOrDefaultAsync(u => u.Id == roleId);
                if (role == null)
                {
                    throw new Exception("用戶不存在");
                }

                Console.WriteLine($"找到用戶: {role.RoleName}");

                // 移除現有角色
                var existingRoles = _context.AdminRoleFunctionRels.Where(ur => ur.RoleId == roleId).ToList();
                Console.WriteLine($"現有角色數量: {existingRoles.Count}");
                _context.AdminRoleFunctionRels.RemoveRange(existingRoles);

                // 添加新角色
                foreach (var funcId in assignedFunctionIds)
                {
                    if (!_context.AdminRoles.Any(r => r.Id == funcId))
                    {
                        throw new Exception($"角色 ID {funcId} 不存在");
                    }

                    _context.AdminRoleFunctionRels.Add(new AdminRoleFunctionRel
                    {
                        RoleId = roleId,
                        FunctionId= funcId
                    });
                    Console.WriteLine($"添加角色: RoleId = {roleId}");
                }

                // 保存更改
                await _context.SaveChangesAsync();
                Console.WriteLine("角色更新成功");
            }
            catch (DbUpdateException dbEx)
            {
                var detailedMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"資料庫更新失敗: {detailedMessage}");
                throw new Exception($"資料庫更新失敗: {detailedMessage}", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新角色時發生一般錯誤: {ex.Message}");
                throw new Exception($"更新角色失敗: {ex.Message}", ex);
            }
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

        //新增權限
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

        // 獲取功能是否已存在
        public async Task<AdminRoleFunction> GetFunctionByNameAsync(string functionName)
        {
            return await _context.AdminRoleFunctions.FirstOrDefaultAsync(f => f.FunctionName == functionName);
        }

        // 添加新功能
        public async Task<AdminRoleFunction> AddFunctionAsync(string functionName)
        {
            var function = new AdminRoleFunction { FunctionName = functionName };
            await _context.AdminRoleFunctions.AddAsync(function);
            await _context.SaveChangesAsync();
            return function;
        }

        // 為功能分配角色
        public async Task AssignRoleToFunctionAsync(int functionId, int roleId)
        {
            var rel = new AdminRoleFunctionRel
            {
                FunctionId = functionId,
                RoleId = roleId
            };
            await _context.AdminRoleFunctionRels.AddAsync(rel);
            await _context.SaveChangesAsync();
        }



    }


}

