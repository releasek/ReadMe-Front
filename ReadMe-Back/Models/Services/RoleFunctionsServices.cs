using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.Repositories;

namespace ReadMe_Back.Models.Services
{
    public class RoleFunctionsServices
    {
        private readonly RoleFunctionsEFRepo _repo;

        public RoleFunctionsServices(RoleFunctionsEFRepo repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        // 獲取所有角色
        public async Task<List<RoleFunctionDto>> GetAllRolesAsync()
        {
            // 從 Repository 獲取資料
            var roles = await _repo.GetAllRolesAsync();

            // 映射為 DTO
            return roles.Select(role => new RoleFunctionDto
            {
                Id = role.Id,
                RoleName = role.RoleName
            }).ToList();
        }
        // 獲取已分配的功能
        public async Task<List<RoleFunctionDto>> GetAssignedFunctionsAsync(int roleId)
        {
            var functions = await _repo.GetAssignedFunctionsAsync(roleId);
            return functions.Select(f => new RoleFunctionDto
            {
                Id = f.Id,
                FunctionName = f.FunctionName
            }).ToList();
        }
        // 獲取未分配的功能
        public async Task<List<RoleFunctionDto>> GetUnassignedFunctionsAsync(int roleId)
        {
            var functions = await _repo.GetUnassignedFunctionsAsync(roleId);
            return functions.Select(f => new RoleFunctionDto
            {
                Id = f.Id,
                FunctionName = f.FunctionName
            }).ToList();
        }

        // 更新角色的功能
        public async Task UpdateRoleFunctionsAsync(int roleId, List<int> assignedFunctionIds)
        {
            await _repo.UpdateRoleFunctionsAsync(roleId, assignedFunctionIds);
        }
        

        //-----------------------------------------------------------------
        //建立新的角色
        public async Task<List<RoleFunctionDto>> GetAllFunctionsAsync()
        {
            var functions = await _repo.GetAllFunctionsAsync();
            return functions.Select(f => new RoleFunctionDto
            {
                Id = f.Id,
                FunctionName = f.FunctionName
            }).ToList();
        }


        public async Task CreateRoleAsync(string roleName, List<int> assignedFunctionIds)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("角色名稱不能為空");
            }

            try
            {
                // 檢查角色是否存在
                var existingRole = await _repo.GetRoleByNameAsync(roleName);
                if (existingRole != null)
                {
                    throw new Exception($"角色名稱 '{roleName}' 已存在");
                }

                // 創建新角色
                var newRole = await _repo.AddRoleAsync(roleName);

                // 為角色分配功能
                if (assignedFunctionIds != null && assignedFunctionIds.Any())
                {
                    foreach (var functionId in assignedFunctionIds)
                    {
                        await _repo.AddRoleFunctionAsync(newRole.Id, functionId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"新增角色失敗: {ex.Message}");
                throw;
            }
        }





    }
}
