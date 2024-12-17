using SchoolHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interface
{
    public interface IDisciplinaRepository
    {
        Task<IEnumerable<Disciplina>> GetAllAsync(Guid tennantId);
        Task<Disciplina> GetByIdAsync(Guid id);
        Task<Disciplina> CreateAsync(Disciplina disciplina);
        Task<Disciplina> UpdateAsync(Disciplina disciplina);
        Task<Disciplina> DeleteAsync(Guid id);
    }
}
