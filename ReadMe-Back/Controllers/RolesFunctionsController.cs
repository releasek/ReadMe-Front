using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Controllers
{
    public class RolesFunctionsController : Controller
    {
        private readonly AppDbContext _context;

        public RolesFunctionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RolesFunctions
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminRoles.ToListAsync());
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
