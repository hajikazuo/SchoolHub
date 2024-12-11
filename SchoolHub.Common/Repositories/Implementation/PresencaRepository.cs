using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models;
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
    }
}
