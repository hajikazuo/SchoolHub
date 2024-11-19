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
using SchoolHub.Common.Repositories.Interfaces;
using SchoolHub.Mvc.Extensions;

namespace SchoolHub.Mvc.Controllers
{
    public class ClassGroupsController : Controller
    {
        private readonly IClassGroupRepository _classGroupRepository;
        public ClassGroupsController(IClassGroupRepository classGroupRepository, ICombProvider comb, AppDbContext context, UserManager<User> userManager) : base(comb, context, userManager)
        {
            _classGroupRepository = classGroupRepository;
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
    }
}
