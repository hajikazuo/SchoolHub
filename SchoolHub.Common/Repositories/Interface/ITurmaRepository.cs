using SchoolHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interface
{
    public interface ITurmaRepository
    {
        Task<IEnumerable<Turma>> GetAllAsync(Guid tennantId);
        Task<Turma> GetByIdAsync(Guid id);
        Task<Turma> CreateAsync(Turma turma);
        Task<Turma> UpdateAsync(Turma turma);
        Task<Turma> DeleteAsync(Guid id);
    }
}
