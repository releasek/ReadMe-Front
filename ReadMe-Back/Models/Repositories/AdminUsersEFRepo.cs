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
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        // 獲取用戶已分配的角色
        public async Task<List<RoleDto>> GetAssignedRolesAsync(int userId)
        {
            return await _context.AdminUserRoleRels
                .Where(rel => rel.UserId == userId)
                .Select(rel => new RoleDto
                {
                    Id = rel.RoleId,
                    RoleName = rel.Role.RoleName
                })
                .ToListAsync();
        }

        // 獲取用戶未分配的角色
        public async Task<List<RoleDto>> GetUnassignedRolesAsync(int userId)
        {
            var assignedRoleIds = await _context.AdminUserRoleRels
                .Where(rel => rel.UserId == userId)
                .Select(rel => rel.RoleId)
                .ToListAsync();

            return await _context.AdminRoles
                .Where(role => !assignedRoleIds.Contains(role.Id))
                .Select(role => new RoleDto
                {
                    Id = role.Id,
                    RoleName = role.RoleName
                })
                .ToListAsync();
        }

        // 更新用戶角色
        public async Task UpdateUserRolesAsync(int userId, List<int> assignedRoleIds)
        {
            Console.WriteLine($"開始更新用戶角色: UserId = {userId}, AssignedRoleIds = [{string.Join(", ", assignedRoleIds)}]");

            if (userId <= 0 || assignedRoleIds == null || !assignedRoleIds.Any())
            {
                throw new ArgumentException("無效的參數");
            }

            try
            {
                // 確認用戶是否存在
                var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("用戶不存在");
                }

                Console.WriteLine($"找到用戶: {user.UserName}");

                // 移除現有角色
                var existingRoles = _context.AdminUserRoleRels.Where(ur => ur.UserId == userId).ToList();
                Console.WriteLine($"現有角色數量: {existingRoles.Count}");
                _context.AdminUserRoleRels.RemoveRange(existingRoles);

                // 添加新角色
                foreach (var roleId in assignedRoleIds)
                {
                    if (!_context.AdminRoles.Any(r => r.Id == roleId))
                    {
                        throw new Exception($"角色 ID {roleId} 不存在");
                    }

                    _context.AdminUserRoleRels.Add(new AdminUserRoleRel
                    {
                        UserId = userId,
                        RoleId = roleId
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

    }
}
