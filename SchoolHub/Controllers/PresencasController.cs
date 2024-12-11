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
            var currentUser = await _userManager.GetUserAsync(User);
            var turmaIdDoUsuario = currentUser.TurmaId;
            var dataAtual = DateTime.Today;

            if (dataFiltro == null)
            {
                dataFiltro = dataAtual;
            }

            var presencasNaTurma = await _presencaRepository.GetAllAsync(turmaIdDoUsuario, dataFiltro.Value);

            ViewBag.DataAtual = dataAtual;
            ViewBag.DataFiltro = dataFiltro;
            return View(presencasNaTurma);
        }

        public async Task<IActionResult> Create(DateTime dataAula)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var turmaId = currentUser.TurmaId;

            if(turmaId == null)
            {
                return NotFound();
            }

            var turma = await _turmaRepository.GetByIdAsync((Guid)turmaId);
            ViewData["DataAula"] = dataAula;
            ViewData["Status"] = this.MontarSelectListParaEnum(new PresencaStatus());
            return View(turma);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DateTime dataAula, [FromForm] Dictionary<string, string> presenca)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var turmaId = currentUser.TurmaId;

            if (turmaId == null)
            {
                return NotFound();
            }

            foreach (var (usuarioId, statusStr) in presenca)
            {
                if (Guid.TryParse(usuarioId, out var parsedGuid) &&
                    Enum.TryParse<PresencaStatus>(statusStr, out var status))
                {
                    var novaPresenca = new Presenca
                    {
                        DataAula = dataAula,
                        Status = status,
                        TurmaId = (Guid)turmaId,
                        UsuarioId = parsedGuid
                    };

                    _context.Presencas.Add(novaPresenca);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
