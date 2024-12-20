using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models;
using SchoolHub.Common.Repositories.Interface;

namespace SchoolHub.Common.Repositories.Implementation
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly AppDbContext _context;

        public TurmaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Turma>> GetAllAsync(Guid tennantId)
        {
            return await _context.Turmas.Where(t => t.TennantId == tennantId).ToListAsync();
        }

        public async Task<Turma> GetByIdAsync(Guid id)
        {
            return await _context.Turmas
                .Include(t => t.Tennant)
                .Include(u => u.Usuarios)
                .Include(d => d.Disciplinas)
                .FirstOrDefaultAsync(c => c.TurmaId == id);
        }

        public async Task<Turma> CreateAsync(Turma turma)
        {
            await _context.Turmas.AddAsync(turma);
            await _context.SaveChangesAsync();

            return turma;
        }

        public async Task<Turma> UpdateAsync(Turma turma)
        {
            var turmaExistente = await _context.Turmas
                .Include(t => t.Disciplinas)
                .FirstOrDefaultAsync(c => c.TurmaId == turma.TurmaId);

            if (turmaExistente is null)
            {
                return null;
            }

            _context.Entry(turmaExistente).CurrentValues.SetValues(turma);

            var disciplinaIds = turma.Disciplinas.Select(d => d.DisciplinaId).ToList();

            turmaExistente.Disciplinas = await _context.Disciplinas
                .Where(d => disciplinaIds.Contains(d.DisciplinaId))
                .ToListAsync();

            await _context.SaveChangesAsync();
            return turmaExistente;
        }

        public async Task<Turma> DeleteAsync(Guid id)
        {
            var turmaExistente = await _context.Turmas.FirstOrDefaultAsync(c => c.TurmaId == id);
            if (turmaExistente is null)
            {
                return null;
            }

            _context.Turmas.Remove(turmaExistente);
            await _context.SaveChangesAsync();
            return turmaExistente;
        }

        public async Task<List<Disciplina>> GetDisciplinas(List<Guid> disciplinaIds)
        {
            return await _context.Disciplinas
                            .Where(d => disciplinaIds.Contains(d.DisciplinaId))
                            .ToListAsync();
        }

        public async Task<bool> AdicionarUsuariosATurma(Guid turmaId, List<Guid> usuariosParaAdd)
        {
            var turma = await _context.Turmas.FindAsync(turmaId);
            if (turma == null)
            {
                return false;
            }

            var usuarios = await _context.Usuarios.Where(u => usuariosParaAdd.Contains(u.Id)).ToListAsync();

            foreach (var usuario in usuarios)
            {
                usuario.TurmaId = turma.TurmaId;
            }

            _context.UpdateRange(usuarios);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoverUsuariosDaTurma(Guid turmaId, List<Guid> usuariosParaRemover)
        {
            var turma = await _context.Turmas.FindAsync(turmaId);
            if (turma == null)
            {
                return false;
            }

            var usuarios = await _context.Usuarios.Where(u => usuariosParaRemover.Contains(u.Id)).ToListAsync();

            foreach (var usuario in usuarios)
            {
                usuario.TurmaId = null;
            }

            _context.UpdateRange(usuarios);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
