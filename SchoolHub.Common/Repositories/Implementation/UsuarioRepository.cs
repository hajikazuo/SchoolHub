using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Interface;
using SchoolHub.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Repositories.Implementation
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly AppDbContext _context;

        public UsuarioRepository(UserManager<Usuario> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<List<Usuario>> GetAllAsync(Guid tennantId)
        {
            return await _context.Usuarios.Where(t => t.TennantId == tennantId).ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IdentityResult> CreateAsync(Usuario usuario, string password)
        {
            return await _userManager.CreateAsync(usuario, password);
        }

        public async Task<IdentityResult> UpdateAsync(Usuario usuario)
        {
            return await _userManager.UpdateAsync(usuario);
        }

        public async Task<IdentityResult> DeleteAsync(Usuario usuario)
        {
            return await _userManager.DeleteAsync(usuario);
        }

        public async Task<List<SelectListaGenericaViewModel>> GetAllRolesAsync()
        {
            return await _context.Roles.Select(role => new SelectListaGenericaViewModel
            {
                Id = role.Name,
                Nome = role.Name
            }).ToListAsync();
        }
    }
}
