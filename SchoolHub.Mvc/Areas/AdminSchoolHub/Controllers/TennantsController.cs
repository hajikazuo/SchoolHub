using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities;
using SchoolHub.Common.Models.Entities.Enum;
using SchoolHub.Common.Repositories.Interfaces;
using SchoolHub.Mvc.Extensions;

namespace SchoolHub.Mvc.Areas.AdminSchoolHub.Controllers
{
    [Area("AdminSchoolHub")]
    public class TennantsController : Controller
    {
        private readonly ITennantRepository _tennantRepository;

        public TennantsController(ICombProvider comb, AppDbContext context, ITennantRepository tennantRepository) : base(comb, context)
        {
            _tennantRepository = tennantRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tennants = await _tennantRepository.GetAllAsync();
            return View(tennants);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tennant = await _tennantRepository.GetByIdAsync(id.Value);

            if (tennant == null)
            {
                return NotFound();
            }

            return View(tennant);
        }

        public IActionResult Create()
        {
            ViewData["Status"] = this.AssembleSelectListToEnum(new TennantStatus());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tennant tennant)
        {
            if (ModelState.IsValid)
            {
                tennant.TennantId = _comb.Create();
                await _tennantRepository.CreateAsync(tennant);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Status"] = this.AssembleSelectListToEnum(new TennantStatus());
            return View(tennant);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tennant = await _tennantRepository.GetByIdAsync(id.Value);

            if (tennant == null)
            {
                return NotFound();
            }
            return View(tennant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Tennant tennant)
        {
            if (id != tennant.TennantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tennantRepository.UpdateAsync(tennant);
                return RedirectToAction(nameof(Index));
            }

            return View(tennant);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tennant = await _tennantRepository.GetByIdAsync(id.Value);

            if (tennant == null)
            {
                return NotFound();
            }

            return View(tennant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _tennantRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
