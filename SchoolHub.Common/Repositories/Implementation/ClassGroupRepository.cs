using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities;
using SchoolHub.Common.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Implementation
{
    public class ClassGroupRepository : IClassGroupRepository
    {
        private readonly AppDbContext _context;

        public ClassGroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassGroup>> GetAllAsync(Guid tennantId)
        {
            return await _context.ClassGroups.Where(t => t.TennantId == tennantId).ToListAsync();
        }

        public async Task<ClassGroup> GetByIdAsync(Guid id)
        {
            return await _context.ClassGroups
                .Include(t => t.Tennant)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(c => c.ClassGroupId == id);
        }

        public async Task<ClassGroup> CreateAsync(ClassGroup classGroup)
        {
            await _context.ClassGroups.AddAsync(classGroup);
            await _context.SaveChangesAsync();

            return classGroup;
        }

        public async Task<ClassGroup> UpdateAsync(ClassGroup classGroup)
        {
            var existingClassGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.ClassGroupId == classGroup.ClassGroupId);

            if (existingClassGroup is null)
            {
                return null;
            }

            _context.Entry(existingClassGroup).CurrentValues.SetValues(classGroup);
            await _context.SaveChangesAsync();
            return existingClassGroup;
        }

        public async Task<ClassGroup> DeleteAsync(Guid id)
        {
            var existingClassGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.ClassGroupId == id);
            if (existingClassGroup is null)
            {
                return null;
            }

            _context.ClassGroups.Remove(existingClassGroup);
            await _context.SaveChangesAsync();
            return existingClassGroup;
        }
    }
}
