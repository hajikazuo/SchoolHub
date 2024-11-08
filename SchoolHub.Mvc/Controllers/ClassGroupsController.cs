using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities;
using SchoolHub.Common.Models.Entities.Users;

namespace SchoolHub.Mvc.Controllers
{
    public class ClassGroupsController : Controller
    {
        public ClassGroupsController(ICombProvider comb, AppDbContext context, UserManager<User> userManager) : base(comb, context, userManager)
        {
        }

        // GET: ClassGroups
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ClassGroups.Include(c => c.Tennant);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ClassGroups/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _context.ClassGroups
                .Include(c => c.Tennant)
                .FirstOrDefaultAsync(m => m.ClassGroupId == id);
            if (classGroup == null)
            {
                return NotFound();
            }

            return View(classGroup);
        }

        // GET: ClassGroups/Create
        public IActionResult Create()
        {
            ViewData["TennantId"] = new SelectList(_context.Tennants, "TennantId", "Name");
            return View();
        }

        // POST: ClassGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassGroupId,Name,TennantId,DateRegistration")] ClassGroup classGroup)
        {
            if (ModelState.IsValid)
            {
                classGroup.ClassGroupId = Guid.NewGuid();
                _context.Add(classGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TennantId"] = new SelectList(_context.Tennants, "TennantId", "Name", classGroup.TennantId);
            return View(classGroup);
        }

        // GET: ClassGroups/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _context.ClassGroups.FindAsync(id);
            if (classGroup == null)
            {
                return NotFound();
            }
            ViewData["TennantId"] = new SelectList(_context.Tennants, "TennantId", "Name", classGroup.TennantId);
            return View(classGroup);
        }

        // POST: ClassGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ClassGroupId,Name,TennantId,DateRegistration")] ClassGroup classGroup)
        {
            if (id != classGroup.ClassGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassGroupExists(classGroup.ClassGroupId))
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
            ViewData["TennantId"] = new SelectList(_context.Tennants, "TennantId", "Name", classGroup.TennantId);
            return View(classGroup);
        }

        // GET: ClassGroups/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _context.ClassGroups
                .Include(c => c.Tennant)
                .FirstOrDefaultAsync(m => m.ClassGroupId == id);
            if (classGroup == null)
            {
                return NotFound();
            }

            return View(classGroup);
        }

        // POST: ClassGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var classGroup = await _context.ClassGroups.FindAsync(id);
            if (classGroup != null)
            {
                _context.ClassGroups.Remove(classGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassGroupExists(Guid id)
        {
            return _context.ClassGroups.Any(e => e.ClassGroupId == id);
        }
    }
}
