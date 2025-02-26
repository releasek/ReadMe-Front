using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.Security;
using ReadMe_Back.Models.Services;
using ReadMe_Back.Models.ViewModels;
using System.Security.Claims;

namespace ReadMe_Back.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AdminUsersServices _service;
        private readonly AdminUsersEFRepo _repo;


        // 單一建構函數，注入兩個依賴
        public AdminUsersController(AppDbContext context, AdminUsersServices service, AdminUsersEFRepo repo)
        {
            _context = context;
            _service = service;
            _repo = repo;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")                   
                });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            return RedirectToAction("OrderIndex", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm, string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(vm);

            var user = await _repo.GetUser(vm.Account, vm.Password);
            if (user == null)
            {
                ModelState.AddModelError("Account", "帳號或密碼有誤"); // 寫Account是要決定ErrorMessage要顯示在哪裡
                return View(vm);
            }

            var rights = _context.AdminUserRoleRels
               .Where(x => x.UserId == user.Id)
               .SelectMany(ur => ur.Role.AdminRoleFunctionRels)
               .Select(rf => new { rf.FunctionId, rf.Function.FunctionName })
               .Distinct()
               .ToList();

            var calims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, vm.Account)
            };

            foreach (var right in rights)
            {
                //calims.Add(new Claim(ClaimTypes.Role, right.FunctionName));
                calims.Add(new Claim("function", right.FunctionName));
            }

            var calimsIdentity = new ClaimsIdentity(calims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(calimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);


            // 如果 returnUrl 是預設值，設定為目標頁面
            if (returnUrl == "/")
            {
                returnUrl = "/Home/OrderIndex";
            }

            // 跳轉至指定頁面
            return Redirect(returnUrl);


        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [FunctionAuthorize("權限管理")]
        // GET: AdminUsers
        public async Task<IActionResult> Index()
        {
            // 獲取 DTO 資料
            var adminUserDtos = await _service.GetAllAdminUsersAsync();

            // 映射為 ViewModel
            var adminUserVms = adminUserDtos.Select(dto => new AdminUserVm
            {
                Id = dto.Id,
                UserName = dto.UserName
            }).ToList();

            // 將 ViewModel 傳遞給 View
            return View(adminUserVms);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(int userId)
        {
            try
            {
                var assignedRoles = await _service.GetAssignedRolesAsync(userId); // 已分配角色
                var unassignedRoles = await _service.GetUnassignedRolesAsync(userId); // 未分配角色

                return Json(new { assignedRoles, unassignedRoles });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving roles: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRoles([FromBody] UpdateRolesDto dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.AssignedRoleIds == null)
            {
                return BadRequest(new { message = "請求參數無效" });
            }

            Console.WriteLine($"接收到的 UserId: {dto.UserId}");
            Console.WriteLine($"接收到的 AssignedRoleIds: {string.Join(", ", dto.AssignedRoleIds)}");

            try
            {
                await _service.UpdateRolesAsync(dto.UserId, dto.AssignedRoleIds);
                return Ok(new { message = "角色更新成功" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新角色時發生錯誤: {ex.Message}");
                return StatusCode(500, new { message = "角色更新失敗", error = ex.Message });
            }
        }


        [HttpGet]
        [Route("RolesFunctions/GetAllRolesForPermissions")]
        public async Task<IActionResult> GetAllRolesForPermissions()
        {
            try
            {
                // 檢查資料庫中的角色是否存在
                var roles = await _context.AdminRoles
                    .Select(r => new { r.Id, r.RoleName }) // 確保欄位名稱與資料庫一致
                    .ToListAsync();

                // 如果沒有角色，檢查資料是否為空
                if (!roles.Any())
                {
                    Console.WriteLine("角色列表為空");
                }

                return Ok(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"取得角色列表時發生錯誤：{ex.Message}");
                return StatusCode(500, new { message = "取得角色列表失敗", error = ex.Message });
            }
        }




        [HttpPost]
        [Route("AdminUsers/CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || dto.Password == null)
            {
                return BadRequest(new { message = "請求參數無效" });
            }

            try
            {
                await _service.CreateUserAsync(dto.UserName, dto.Password);
                return Ok(new { message = "使用者新增成功" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"新增使用者失敗: {ex.Message}");
                return StatusCode(500, new { message = "使用者新增失敗", error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            var user = _repo.GetAdminUserById(id);
            if (user != null)
            {
                result = true;
                _repo.DeleteUser(user);
            }
            return Json(result);
        }



        // GET: AdminUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        // GET: AdminUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName")] AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminUser);
        }

        // GET: AdminUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser == null)
            {
                return NotFound();
            }
            return View(adminUser);
        }

        // POST: AdminUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName")] AdminUser adminUser)
        {
            if (id != adminUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminUserExists(adminUser.Id))
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
            return View(adminUser);
        }

        // GET: AdminUsers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var adminUser = await _context.AdminUsers
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (adminUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(adminUser);
        //}

        // POST: AdminUsers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var adminUser = await _context.AdminUsers.FindAsync(id);
        //    if (adminUser != null)
        //    {
        //        _context.AdminUsers.Remove(adminUser);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool AdminUserExists(int id)
        {
            return _context.AdminUsers.Any(e => e.Id == id);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
