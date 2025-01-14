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
        //新增角色
        public async Task CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("角色名稱不能為空");
            }

            // 1. 檢查使用者是否已存在
            var existingUser = await _repo.GetRoleByNameAsync(roleName);
            if (existingUser != null)
            {
                throw new Exception($"角色名稱 '{roleName}' 已存在");
            }

            // 2. 建立新使用者 (存到 AdminUsers)
            var newUser = await _repo.AddRoleAsync(roleName);

        }


        //-----------------------------------------------------------------
        //建立新的功能
        public async Task<List<RoleFunctionDto>> GetAllFunctionsAsync()
        {
            var functions = await _repo.GetAllFunctionsAsync();
            return functions.Select(f => new RoleFunctionDto
            {
                Id = f.Id,
                FunctionName = f.FunctionName
            }).ToList();
        }


        public async Task CreateFunctionAsync(string functionName, List<int> assignedRoleIds)
        {
            if (string.IsNullOrWhiteSpace(functionName))
            {
                throw new ArgumentException("功能名稱不能為空");
            }

            // 檢查功能是否已存在
            var existingFunction = await _repo.GetFunctionByNameAsync(functionName);
            if (existingFunction != null)
            {
                throw new Exception($"功能名稱 '{functionName}' 已存在");
            }

            // 創建新功能
            var newFunction = await _repo.AddFunctionAsync(functionName);

            // 為新功能分配角色
            if (assignedRoleIds != null && assignedRoleIds.Any())
            {
                foreach (var roleId in assignedRoleIds)
                {
                    await _repo.AssignRoleToFunctionAsync(newFunction.Id, roleId);
                }
            }
        }





    }
}
