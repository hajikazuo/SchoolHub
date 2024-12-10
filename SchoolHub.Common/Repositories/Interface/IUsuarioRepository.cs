using Microsoft.AspNetCore.Identity;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Interface
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync(Guid tennantId);
        Task<Usuario> GetByIdAsync(Guid id);
        Task<IdentityResult> CreateAsync(Usuario usuario, string password);
        Task<IdentityResult> UpdateAsync(Usuario usuario);
        Task<IdentityResult> DeleteAsync(Usuario usuario);

        Task<List<SelectListaGenericaViewModel>> GetAllRolesAsync();
    }
}
