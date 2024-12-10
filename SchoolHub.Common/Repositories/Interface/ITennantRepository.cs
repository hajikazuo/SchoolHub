using SchoolHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interface
{
    public interface ITennantRepository
    {
        Task<IEnumerable<Tennant>> GetAllAsync();
        Task<Tennant> GetByIdAsync(Guid id);
        Task<Tennant> CreateAsync(Tennant tennant);
        Task<Tennant> UpdateAsync(Tennant tennant);
        Task<Tennant> DeleteAsync(Guid id);
    }
}
