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
    public class TennantRepository : ITennantRepository
    {
        private readonly AppDbContext _context;

        public TennantRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Tennant>> GetAllAsync()
        {
            return await _context.Tennants.ToListAsync();
        }

        public async Task<Tennant> GetByIdAsync(Guid id)
        {
            return await _context.Tennants.FirstOrDefaultAsync(t => t.TennantId == id);
        }

        public async Task<Tennant> CreateAsync(Tennant tennant)
        {
            await _context.Tennants.AddAsync(tennant);
            await _context.SaveChangesAsync();

            return tennant;
        }

        public async Task<Tennant> UpdateAsync(Tennant tennant)
        {
            var tennantExistente = await _context.Tennants.FirstOrDefaultAsync(t => t.TennantId == tennant.TennantId);

            if (tennantExistente is null)
            {
                return null;
            }

            _context.Entry(tennantExistente).CurrentValues.SetValues(tennant);
            await _context.SaveChangesAsync();
            return tennantExistente;
        }

        public async Task<Tennant> DeleteAsync(Guid id)
        {
            var tennantExistente = await _context.Tennants.FirstOrDefaultAsync(t => t.TennantId == id);
            if (tennantExistente is null)
            {
                return null;
            }

            _context.Tennants.Remove(tennantExistente);
            await _context.SaveChangesAsync();
            return tennantExistente;
        }
    }
}
