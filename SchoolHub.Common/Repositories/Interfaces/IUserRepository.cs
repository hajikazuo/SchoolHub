using Microsoft.AspNetCore.Identity;
using SchoolHub.Common.Models.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(Guid tennantId);
        Task<List<User>> GetUsersWithoutClass(Guid tennantId);
        Task<User> GetByIdAsync(Guid id);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
    }
}
