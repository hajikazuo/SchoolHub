using SchoolHub.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interfaces
{
    public interface IClassGroupRepository
    {
        Task<IEnumerable<ClassGroup>> GetAllAsync(Guid tennantId);
        Task<ClassGroup> GetByIdAsync(Guid id);
        Task<ClassGroup> CreateAsync(ClassGroup classGroup);
        Task<ClassGroup> UpdateAsync(ClassGroup classGroup);
        Task<ClassGroup> DeleteAsync(Guid id);
    }
}
