using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Enums;
using SchoolHub.Common.Models;
using SchoolHub.Common.Repositories.Interface;
using SchoolHub.Mvc.Extensions;
using SchoolHub.Mvc.Services.Interface;
using SchoolHub.Common.Models.Usuarios;

namespace SchoolHub.Mvc.Areas.AdminSchoolHub.Controllers
{
    public class TennantsController : Controller
    {
        private readonly ITennantRepository _tennantRepository;
        private readonly IUploadService _uploadService;
        public TennantsController(ITennantRepository tennantRepository, IUploadService uploadService, ICombProvider comb, AppDbContext context) : base(comb, context)
        {
            _tennantRepository = tennantRepository;
            _uploadService = uploadService;
        }

        public async Task<IActionResult> Index()
        {
            var tennants = await _tennantRepository.GetAllAsync();
            ViewBag.Confirm = TempData["Confirm"];
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
            ViewData["Status"] = this.MontarSelectListParaEnum(new TennantStatus());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tennant tennant, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                tennant.TennantId = _comb.Create();
                tennant.Logo = await _uploadService.UploadFoto(file, PastaUpload.LogoTennant);
                await _tennantRepository.CreateAsync(tennant);
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Cadastrado com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Status"] = this.MontarSelectListParaEnum(new TennantStatus());
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
        public async Task<IActionResult> Edit(Guid id, Tennant tennant, IFormFile? file)
        {
            if (id != tennant.TennantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    tennant.Logo = await _uploadService.UploadFoto(file, PastaUpload.LogoTennant);
                }

                await _tennantRepository.UpdateAsync(tennant);
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Atualizado com sucesso!');})</script>";
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
            TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Excluído com sucesso!');})</script>";
            return RedirectToAction(nameof(Index));
        }
    }
}
