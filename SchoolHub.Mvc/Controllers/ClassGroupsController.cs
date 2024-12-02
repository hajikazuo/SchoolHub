using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities;
using SchoolHub.Common.Models.Entities.Users;
using SchoolHub.Common.Repositories.Interfaces;
using SchoolHub.Mvc.Extensions;
using SchoolHub.Mvc.ViewModels;

namespace SchoolHub.Mvc.Controllers
{
    public class ClassGroupsController : Controller
    {
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly IUserRepository _userRepository;

        public ClassGroupsController(IClassGroupRepository classGroupRepository, IUserRepository userRepository, ICombProvider comb, AppDbContext context, UserManager<User> userManager) : base(comb, context, userManager)
        {
            _classGroupRepository = classGroupRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tennantid = this._tennantIdUserLoggedIn;
            var classGroup = await _classGroupRepository.GetAllAsync(tennantid);
            return View(classGroup);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(id.Value);

            if (classGroup == null)
            {
                return NotFound();
            }

            return View(classGroup);
        }

        public async Task<IActionResult> Create()
        {
            var tennantid = this._tennantIdUserLoggedIn;
            var users = await _userRepository.GetAllAsync(tennantid);
            ViewBag.AvailableUsers = users;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassGroup classGroup)
        {
            var tennantid = this._tennantIdUserLoggedIn;

            if (ModelState.IsValid)
            {
                classGroup.ClassGroupId = _comb.Create();
                classGroup.TennantId = tennantid;

                var response = await _classGroupRepository.CreateAsync(classGroup);
                return RedirectToAction(nameof(Index));
            }

            var users = await _userRepository.GetAllAsync(tennantid);
            ViewBag.AvailableUsers = users;
            return View(classGroup);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(id.Value);

            if (classGroup == null)
            {
                return NotFound();
            }

            return View(classGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ClassGroup classGroup)
        {
            if (id != classGroup.ClassGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _classGroupRepository.UpdateAsync(classGroup);

                return RedirectToAction(nameof(Index));
            }
            return View(classGroup);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(id.Value);

            if (classGroup == null)
            {
                return NotFound();
            }

            return View(classGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _classGroupRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddStudents(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(id.Value);

            if (classGroup == null)
            {
                return NotFound();
            }

            var students = await _userRepository.GetUsersWithoutClass(this._tennantIdUserLoggedIn);

            ViewData["Students"] = students;

            return View(classGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudents(Guid id, Guid[] students)
        {
            if (students != null)
            {
                var classGroup = await _classGroupRepository.GetByIdAsync(id);
                if (classGroup == null)
                {
                    return NotFound();
                }

                var studentsToUpdate = await _context.Users.Where(u => students.Contains(u.Id)).ToListAsync();

                foreach (var student in studentsToUpdate)
                {
                    student.ClassGroupId = id;
                }

                _context.Users.UpdateRange(studentsToUpdate);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveStudents(Guid id, Guid[] students)
        {
            if (students != null)
            {
                var classGroup = await _classGroupRepository.GetByIdAsync(id);
                if (classGroup == null)
                {
                    return NotFound();
                }

                var studentsToRemove = await _context.Users
                    .Where(u => students.Contains(u.Id) && u.ClassGroupId == id)
                    .ToListAsync();

                foreach (var student in studentsToRemove)
                {
                    student.ClassGroupId = null; 
                }

                _context.Users.UpdateRange(studentsToRemove);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
