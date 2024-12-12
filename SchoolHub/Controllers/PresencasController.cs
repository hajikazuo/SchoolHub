using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models;
using SchoolHub.Common.Models.Enums;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Implementation;
using SchoolHub.Common.Repositories.Interface;
using SchoolHub.Mvc.Extensions;
using SchoolHub.Mvc.ViewModels;

namespace SchoolHub.Mvc.Controllers
{
    public class PresencasController : Controller
    {
        private readonly IPresencaRepository _presencaRepository;
        private readonly ITurmaRepository _turmaRepository;
        public PresencasController(IPresencaRepository presencaRepository, ITurmaRepository turmaRepository, ICombProvider comb, AppDbContext context, UserManager<Usuario> userManager) : base(comb, context, userManager)
        {
            _presencaRepository = presencaRepository;
            _turmaRepository = turmaRepository;
        }

        public async Task<IActionResult> Index(DateTime? dataFiltro = null)
        {
            var turmaId = _turmaUsuarioLogado;
            var dataAtual = DateTime.Today;

            if (dataFiltro == null)
            {
                dataFiltro = dataAtual;
            }

            var presencasNaTurma = await _presencaRepository.GetAllAsync(turmaId, dataFiltro.Value);

            ViewBag.Confirm = TempData["Confirm"];
            ViewBag.DataAtual = dataAtual;
            ViewBag.DataFiltro = dataFiltro;
            return View(presencasNaTurma);
        }

        public async Task<IActionResult> Create()
        {
            var turmaId = _turmaUsuarioLogado;

            if (turmaId == default)
            {
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraErro('Erro', 'Você precisa estar vinculado à uma turma para acessar essa função');})</script>";
                return RedirectToAction(nameof(Index));
            }

            var turma = await _turmaRepository.GetByIdAsync(turmaId);
            var viewModel = new PresencaViewModel
            {
                DataAula = DateTime.Today,
                Alunos = turma.Usuarios.Select(u => new PresencaAlunoViewModel
                {
                    UsuarioId = u.Id,
                    Nome = u.Nome
                }).ToList()
            };
            ViewData["Status"] = this.MontarSelectListParaEnum(new PresencaStatus());
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PresencaViewModel viewModel)
        {
            var turmaId = _turmaUsuarioLogado;

            if (turmaId == default)
            {
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraErro('Erro', 'Você precisa estar vinculado à uma turma para acessar essa função');})</script>";
                return RedirectToAction(nameof(Index));
            }

            foreach (var aluno in viewModel.Alunos)
            {
                var novaPresenca = new Presenca
                {
                    DataAula = viewModel.DataAula,
                    Status = aluno.Status,
                    TurmaId = turmaId,
                    UsuarioId = aluno.UsuarioId,
                    Observacoes = aluno.Observacoes
                };

                _context.Presencas.Add(novaPresenca);
            }

            await _context.SaveChangesAsync();
            TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Cadastrado com sucesso!');})</script>";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var presenca = await _presencaRepository.GetByIdAsync(id.Value);

            if (presenca == null)
            {
                return NotFound();
            }

            ViewData["Status"] = this.MontarSelectListParaEnum(new PresencaStatus());
            return View(presenca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Presenca presenca)
        {
            if (id != presenca.PresencaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _presencaRepository.UpdateAsync(presenca);

                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Atualizado com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            return View(presenca);
        }

        public async Task<IActionResult> PresencasPorUsuario(Guid? id, DateTime? dataFiltro = null)
        {
            var dataAtual = DateTime.Today;

            if (dataFiltro == null)
            {
                dataFiltro = dataAtual;
            }

            var presencasDoUsuario = await _presencaRepository.GetByUserAsync(id, dataFiltro.Value);

            ViewBag.totalFaltas = presencasDoUsuario.Count(p => p.Status == PresencaStatus.Ausente);
            ViewBag.DataAtual = dataAtual;
            ViewBag.DataFiltro = dataFiltro;

            return View(presencasDoUsuario);
        }
    }
}
