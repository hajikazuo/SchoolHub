using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Implementation;
using SchoolHub.Common.Repositories.Interface;

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

            return View(turma);
        }

        public async Task<IActionResult> Create()
        {
            var tennantid = this._tennantIdUsuarioLogado;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Turma turma)
        {
            var tennantid = this._tennantIdUsuarioLogado;

            if (ModelState.IsValid)
            {
                turma.TurmaId = _comb.Create();
                turma.TennantId = tennantid;

                var response = await _turmaRepository.CreateAsync(turma);
                return RedirectToAction(nameof(Index));
            }
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

            return View(turma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Turma turma)
        {
            if (id != turma.TurmaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _turmaRepository.UpdateAsync(turma);

                return RedirectToAction(nameof(Index));
            }
            return View(turma);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classGroup = await _turmaRepository.GetByIdAsync(id.Value);

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
            await _turmaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarUsuarios(Guid turmaId, List<Guid> usuariosParaAdd)
        {
            var turma = await _context.Turmas.FindAsync(turmaId);
            if (turma == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.Where(u => usuariosParaAdd.Contains(u.Id)).ToListAsync();

            foreach (var usuario in usuarios)
            {
                usuario.TurmaId = turma.TurmaId;     
            }

            _context.UpdateRange(usuarios); 
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = turmaId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverUsuarios(Guid turmaId, List<Guid> usuariosParaRemover)
        {
            var turma = await _context.Turmas.FindAsync(turmaId);

            if (turma == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.Where(u => usuariosParaRemover.Contains(u.Id)).ToListAsync();

            foreach (var usuario in usuarios)
            {
                usuario.TurmaId = null;   
            }
            _context.UpdateRange(usuarios);
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Details), new { id = turmaId }); 
        }
    }
}
