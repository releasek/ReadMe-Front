using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.ViewModels;
using ReadMe_Back.Models.Services;
using ReadMe_Back.Models.DTOs;

namespace ReadMe_Back.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AdminUsersServices _service;

        // 單一建構函數，注入兩個依賴
        public AdminUsersController(AppDbContext context, AdminUsersServices service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVm vm, string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(vm);
            if (vm.Username == "admin" && vm.Password == "123")
            {
                var calims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, vm.Username),
                    new Claim("fullname", "Allen Kuo"),
                    new Claim("function", "home_privacy"),
                    new Claim("function", "products")
                };
                var calimsIdentity = new ClaimsIdentity(calims, "CookieAuth");
                var userPrincipal = new ClaimsPrincipal(new[] { calimsIdentity });
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                return Redirect(returnUrl);
            }
            ModelState.AddModelError(string.Empty, "Account or password is invalid");
            return View(vm);
        }

        // GET: AdminUsers
        public async Task<IActionResult> Index()
        {
            // 獲取 DTO 資料
            var adminUserDtos = await _service.GetAllAdminUsers();

            // 映射為 ViewModel
            var adminUserVms = adminUserDtos.Select(dto => new AdminUserVm
            {
                Id = dto.Id,
                UserName = dto.UserName
            }).ToList();

            // 將 ViewModel 傳遞給 View
            return View(adminUserVms);
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
        public async Task<IActionResult> Delete(int? id)
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

        // POST: AdminUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser != null)
            {
                _context.AdminUsers.Remove(adminUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminUserExists(int id)
        {
            return _context.AdminUsers.Any(e => e.Id == id);
        }
    }
}
