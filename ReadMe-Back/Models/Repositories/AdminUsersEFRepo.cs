using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Models.Repositories
{
    public class AdminUsersEFRepo
    {
        private readonly AppDbContext _context;

        public AdminUsersEFRepo(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<AdminUserDto> GetUser(string account, string password)
        {
            var user = _context.AdminUsers.FirstOrDefault(u => u.UserName == account && u.Password == password);

            if (user == null) return null;

            // 將 AdminUser 轉換為 AdminUserDto
            var userDto = new AdminUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
            };

            return userDto;

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

        public AdminUser GetAdminUserById(int id)
        {
            var user = _context.AdminUsers
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public void DeleteUser(AdminUser user)
        {
            _context.AdminUsers.Remove(user);
            _context.SaveChanges();
        }


        // 獲取用戶已分配的角色
        public async Task<List<CreateRoleDto>> GetAssignedRolesAsync(int userId)
        {
            return await _context.AdminUserRoleRels
                .Where(rel => rel.UserId == userId)
                .Select(rel => new CreateRoleDto
                {
                    Id = rel.RoleId,
                    RoleName = rel.Role.RoleName
                })
                .ToListAsync();
        }

        // 獲取用戶未分配的角色
        public async Task<List<CreateRoleDto>> GetUnassignedRolesAsync(int userId)
        {
            var assignedRoleIds = await _context.AdminUserRoleRels
                .Where(rel => rel.UserId == userId)
                .Select(rel => rel.RoleId)
                .ToListAsync();

            return await _context.AdminRoles
                .Where(role => !assignedRoleIds.Contains(role.Id))
                .Select(role => new CreateRoleDto
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



        //新增使用者
        public async Task<AdminUser> GetUserByNameAsync(string userName)
        {
            return await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
        public async Task<AdminUser> AddUserAsync(string username,string password)
        {
            var newuser = new AdminUser { UserName = username, Password = password };
            _context.AdminUsers.Add(newuser);
            await _context.SaveChangesAsync();
            return newuser;
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            _context.AdminUserRoleRels.Add(new AdminUserRoleRel
            {
                UserId = userId,
                RoleId = roleId
            });
            await _context.SaveChangesAsync();
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

        //// 獲取功能是否已存在
        //public async Task<AdminRoleFunction> GetFunctionByNameAsync(string functionName)
        //{
        //    return await _context.AdminRoleFunctions.FirstOrDefaultAsync(f => f.FunctionName == functionName);
        //}   

        // 添加新角色
        public async Task<AdminRole> AddRoleAsync(string roleName)
        {
            var role = new AdminRole { RoleName = roleName };
            await _context.AdminRoles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
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
