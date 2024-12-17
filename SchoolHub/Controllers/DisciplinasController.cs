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
using SchoolHub.Common.Models;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Interface;

namespace SchoolHub.Mvc.Controllers
{
    public class DisciplinasController : Controller
    {
        private readonly IDisciplinaRepository _disciplinaRepository;
        public DisciplinasController(IDisciplinaRepository disciplinaRepository, ICombProvider comb, AppDbContext context, UserManager<Usuario> userManager) : base(comb, context, userManager)
        {
            _disciplinaRepository = disciplinaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tennantid = this._tennantIdUsuarioLogado;
            var disciplinas = await _disciplinaRepository.GetAllAsync(tennantid);
            ViewBag.Confirm = TempData["Confirm"];
            return View(disciplinas);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _disciplinaRepository.GetByIdAsync(id.Value);

            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                disciplina.DisciplinaId = _comb.Create();
                disciplina.TennantId = this._tennantIdUsuarioLogado;

                await _disciplinaRepository.CreateAsync(disciplina);

                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Cadastrado com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _disciplinaRepository.GetByIdAsync(id.Value);

            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Disciplina disciplina)
        {
            if (id != disciplina.DisciplinaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _disciplinaRepository.UpdateAsync(disciplina);
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Atualizado com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _disciplinaRepository.DeleteAsync(id);
            TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Excluído com sucesso!');})</script>";
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplinaExists(Guid id)
        {
            return _context.Disciplinas.Any(e => e.DisciplinaId == id);
        }
    }
}
