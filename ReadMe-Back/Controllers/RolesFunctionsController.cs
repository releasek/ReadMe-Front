using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Security;
using ReadMe_Back.Models.Services;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Controllers
{
    [FunctionAuthorize("權限管理")]
    public class RolesFunctionsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly RoleFunctionsServices _service;

        public RolesFunctionsController(AppDbContext context, RoleFunctionsServices service)
        {
            _context = context;
            _service = service;
        }

        // GET: RolesFunctions
        public async Task<IActionResult> Index()
        {
            // 獲取 DTO 資料
            var roleDtos = await _service.GetAllRolesAsync();

            // 映射為 ViewModel
            var roleVms = roleDtos.Select(dto => new RoleFunctionVm
            {
                Id = dto.Id,
                RoleName = dto.RoleName
            }).ToList();

            // 傳遞 ViewModel 至 View
            return View(roleVms);
        }
        [HttpGet]
        //傳入Function
        public async Task<IActionResult> GetFunctions(int roleId)
        {

            if (roleId <= 0)
            {
                return BadRequest(new { message = "角色 ID 無效" });
            }

            try
            {
                var assignedFunctions = await _service.GetAssignedFunctionsAsync(roleId);
                var unassignedFunctions = await _service.GetUnassignedFunctionsAsync(roleId);

                return Ok(new
                {
                    assignedFunctions = assignedFunctions.Select(f => new RoleFunctionVm
                    {
                        Id = f.Id,
                        FunctionName = f.FunctionName
                    }),
                    unassignedFunctions = unassignedFunctions.Select(f => new RoleFunctionVm
                    {
                        Id = f.Id,
                        FunctionName = f.FunctionName
                    })
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"取得功能時發生錯誤: {ex.Message}");
                return StatusCode(500, new { message = "取得功能失敗", error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFunctions([FromBody] UpdateFunctionsDto dto)
        {
            if (dto == null || dto.RoleId <= 0 || dto.AssignedFunctionIds == null)
            {
                return BadRequest(new { message = "請求參數無效" });
            }

            Console.WriteLine($"接收到的 RoleId: {dto.RoleId}");
            Console.WriteLine($"接收到的 AssignedFunctionIds: {string.Join(", ", dto.AssignedFunctionIds)}");

            try
            {
                await _service.UpdateRoleFunctionsAsync(dto.RoleId, dto.AssignedFunctionIds);
                return Ok(new { message = "功能更新成功" });
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"參數錯誤: {argEx.Message}");
                return BadRequest(new { message = "參數錯誤", error = argEx.Message });
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"資料庫更新失敗: {dbEx.InnerException?.Message ?? dbEx.Message}");
                return StatusCode(500, new { message = "資料庫更新失敗", error = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"未知錯誤: {ex.Message}");
                return StatusCode(500, new { message = "功能更新失敗", error = ex.Message });
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetAllRolesForPermissions()
        {
            try
            {
                // 確保資料庫查詢無誤
                var roles = await _context.AdminRoles
                    .Select(r => new
                    {
                        Id = r.Id,
                        RoleName = r.RoleName
                    })
                    .ToListAsync();

                return Ok(roles); // 正常返回角色清單
            }
            catch (Exception ex)
            {
                // 記錄例外情況並返回錯誤訊息
                Console.WriteLine($"取得角色列表時發生錯誤：{ex.Message}");
                return StatusCode(500, new { message = "取得角色列表失敗", error = ex.Message });
            }
        }
        [HttpPost]

        public async Task<IActionResult> CreateFunction([FromBody] CreateFunctionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FunctionName) || dto.AssignedRoleIds == null)
            {
                return BadRequest(new { message = "請求參數無效" });
            }

            try
            {
                await _service.CreateFunctionAsync(dto.FunctionName, dto.AssignedRoleIds);
                return Ok(new { message = "功能新增成功" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"新增功能失敗: {ex.Message}");
                return StatusCode(500, new { message = "功能新增失敗", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("RoleFunctions/CreateRole")]
        public async Task<IActionResult> CreateUser([FromBody] CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleName) )
            {
                return BadRequest(new { message = "請求參數無效" });
            }

            try
            {
                await _service.CreateRoleAsync(dto.RoleName);
                return Ok(new { message = "使用者新增成功" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"新增角色失敗: {ex.Message}");
                return StatusCode(500, new { message = "新增角色失敗", error = ex.Message });
            }
        }















        // GET: RolesFunctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminRole = await _context.AdminRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminRole == null)
            {
                return NotFound();
            }

            return View(adminRole);
        }

        // GET: RolesFunctions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RolesFunctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleName")] AdminRole adminRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminRole);
        }

        // GET: RolesFunctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminRole = await _context.AdminRoles.FindAsync(id);
            if (adminRole == null)
            {
                return NotFound();
            }
            return View(adminRole);
        }

        // POST: RolesFunctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoleName")] AdminRole adminRole)
        {
            if (id != adminRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminRoleExists(adminRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adminRole);
        }

        // GET: RolesFunctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminRole = await _context.AdminRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminRole == null)
            {
                return NotFound();
            }

            return View(adminRole);
        }

        // POST: RolesFunctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminRole = await _context.AdminRoles.FindAsync(id);
            if (adminRole != null)
            {
                _context.AdminRoles.Remove(adminRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminRoleExists(int id)
        {
            return _context.AdminRoles.Any(e => e.Id == id);
        }
    }
}
