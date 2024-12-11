using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Implementation
{
    public class PresencaRepository : IPresencaRepository
    {
        private readonly AppDbContext _context;

        public PresencaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Presenca>> GetAllAsync(Guid? turmaId, DateTime dataFiltro)
        {
            return await _context.Presencas
               .Where(p => p.TurmaId == turmaId && p.DataAula.Date == dataFiltro.Date)
               .Include(p => p.Usuario)
               .OrderBy(p => p.DataAula)
               .ToListAsync();
        }

        public async Task<Presenca> GetByIdAsync(Guid id)
        {
            return await _context.Presencas
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.PresencaId == id);
        }

        public async Task<List<Presenca>> GetByUserAsync(Guid? id, DateTime dataFiltro)
        {
            return await _context.Presencas
                .Where(p => p.UsuarioId == id && p.DataAula.Date.Month == dataFiltro.Date.Month)
                .ToListAsync();
        }

        public async Task<Presenca> UpdateAsync(Presenca presenca)
        {
            var presencaExistente = await _context.Presencas.FirstOrDefaultAsync(c => c.PresencaId == presenca.PresencaId);

            if (presencaExistente is null)
            {
                return null;
            }

            _context.Entry(presencaExistente).CurrentValues.SetValues(presenca);
            await _context.SaveChangesAsync();
            return presencaExistente;
        }
    }
}
