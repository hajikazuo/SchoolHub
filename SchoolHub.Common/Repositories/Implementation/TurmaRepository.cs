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
            var turmaExistente = await _context.Turmas.FirstOrDefaultAsync(c => c.TurmaId == turma.TurmaId);

            if (turmaExistente is null)
            {
                return null;
            }

            _context.Entry(turmaExistente).CurrentValues.SetValues(turma);
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
    }
}
