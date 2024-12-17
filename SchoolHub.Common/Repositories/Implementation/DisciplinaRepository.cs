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
    public class DisciplinaRepository : IDisciplinaRepository
    {
        private readonly AppDbContext _context;

        public DisciplinaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Disciplina>> GetAllAsync(Guid tennantId)
        {
            return await _context.Disciplinas.Where(t => t.TennantId == tennantId).ToListAsync();
        }

        public async Task<Disciplina> GetByIdAsync(Guid id)
        {
            return await _context.Disciplinas
                .Include(t => t.Tennant)
                .FirstOrDefaultAsync(c => c.DisciplinaId == id);
        }

        public async Task<Disciplina> CreateAsync(Disciplina disciplina)
        {
            await _context.Disciplinas.AddAsync(disciplina);
            await _context.SaveChangesAsync();

            return disciplina;
        }

        public async Task<Disciplina> UpdateAsync(Disciplina disciplina)
        {
            var disciplinaExistente = await _context.Disciplinas.FirstOrDefaultAsync(c => c.DisciplinaId == disciplina.DisciplinaId);

            if (disciplinaExistente is null)
            {
                return null;
            }

            _context.Entry(disciplinaExistente).CurrentValues.SetValues(disciplina);
            await _context.SaveChangesAsync();
            return disciplinaExistente;
        }

        public async Task<Disciplina> DeleteAsync(Guid id)
        {
            var disciplinaExistente = await _context.Disciplinas.FirstOrDefaultAsync(c => c.DisciplinaId == id);
            if (disciplinaExistente is null)
            {
                return null;
            }

            _context.Disciplinas.Remove(disciplinaExistente);
            await _context.SaveChangesAsync();
            return disciplinaExistente;
        }
    }
}
