using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Migrations;
using SchoolHub.Common.Models;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Implementation;
using SchoolHub.Common.Repositories.Interface;
using SchoolHub.Mvc.Extensions;

namespace SchoolHub.Mvc.Controllers
{
    public class TurmasController : Controller
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TurmasController(ITurmaRepository turmaRepository, IUsuarioRepository usuarioRepository, ICombProvider comb, AppDbContext context, UserManager<Usuario> userManager) : base(comb, context, userManager)
        {
            _turmaRepository = turmaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tennantid = this._tennantIdUsuarioLogado;
            var turma = await _turmaRepository.GetAllAsync(tennantid);
            ViewBag.Confirm = TempData["Confirm"];
            return View(turma);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var tennantid = this._tennantIdUsuarioLogado;

            if (id == null)
            {
                return NotFound();
            }

            var turma = await _turmaRepository.GetByIdAsync(id.Value);

            if (turma == null)
            {
                return NotFound();
            }

            ViewData["UsuariosSemTurma"] = await _context.Users.Where(t => t.TennantId == tennantid && t.Turma == null).ToListAsync();
            ViewBag.Confirm = TempData["Confirm"];
            return View(turma);
        }

        public async Task<IActionResult> Create()
        {
            var tennantid = this._tennantIdUsuarioLogado;

            ViewData["Disciplinas"] = new SelectList(await DropDownFunc.DisciplinaAsync(_context, tennantid), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Turma turma, List<Guid> disciplinaIds)
        {
            var tennantid = this._tennantIdUsuarioLogado;

            if (ModelState.IsValid)
            {
                turma.TurmaId = _comb.Create();
                turma.TennantId = tennantid;
                turma.Disciplinas = await _turmaRepository.GetDisciplinas(disciplinaIds);

                await _turmaRepository.CreateAsync(turma);

                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Cadastrado com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Disciplinas"] = new SelectList(await DropDownFunc.DisciplinaAsync(_context, tennantid), "Id", "Nome");
            return View(turma);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _turmaRepository.GetByIdAsync(id.Value);

            if (turma == null)
            {
                return NotFound();
            }

            ViewData["Disciplinas"] = new SelectList(await DropDownFunc.DisciplinaAsync(_context, turma.TennantId), "Id", "Nome");
            return View(turma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Turma turma, List<Guid> disciplinaIds)
        {
            if (id != turma.TurmaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                turma.Disciplinas = await _turmaRepository.GetDisciplinas(disciplinaIds);

                await _turmaRepository.UpdateAsync(turma);

                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Atualizado com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            return View(turma);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _turmaRepository.DeleteAsync(id);
            TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Excluído com sucesso!');})</script>";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarUsuarios(Guid turmaId, List<Guid> usuariosParaAdd)
        {
            var sucesso = await _turmaRepository.AdicionarUsuariosATurma(turmaId, usuariosParaAdd);

            if (!sucesso)
            {
                return NotFound();
            }

            TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Usuários adicionados com sucesso!');})</script>";
            return RedirectToAction(nameof(Details), new { id = turmaId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverUsuarios(Guid turmaId, List<Guid> usuariosParaRemover)
        {
            var sucesso = await _turmaRepository.RemoverUsuariosDaTurma(turmaId, usuariosParaRemover);

            if (!sucesso)
            {
                return NotFound();
            }

            TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Usuários removidos com sucesso!');})</script>";
            return RedirectToAction(nameof(Details), new { id = turmaId });
        }
    }
}
