using SchoolHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interface
{
    public interface IPresencaRepository
    {
        Task<List<Presenca>> GetAllAsync(Guid? turmaId, DateTime dataFiltro);
    }
}
