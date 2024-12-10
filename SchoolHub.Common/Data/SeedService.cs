using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Models.Enums;
using SchoolHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolHub.Common.Models.Usuarios;

namespace SchoolHub.Common.Data
{
    public class SeedService : ISeedService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Funcao> _roleManager;
        private readonly AppDbContext _context;

        private const string Admin = "Admin";
        private const string Professor = "Professor";
        private const string Aluno = "Aluno";

        private Guid TennantIDModel = new Guid("3b7b2c08-601d-46c9-b047-32f8c601649b");

        public SeedService(UserManager<Usuario> userManager, RoleManager<Funcao> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Seed()
        {
            CreateTennant().GetAwaiter().GetResult();
            CreateRole(Admin).GetAwaiter().GetResult();
            CreateRole(Professor).GetAwaiter().GetResult();
            CreateRole(Aluno).GetAwaiter().GetResult();
            CreateUser("admin@escola.com.br", name: "Administrador", "Teste@2024", roles: new List<string> { Admin, Professor, Aluno }, TennantIDModel).GetAwaiter().GetResult();
        }

        private async Task<IdentityResult> CreateUser(string email, string name, string password, IEnumerable<string> roles, Guid? tennantId = null)
        {
            var retorno = await _userManager.FindByEmailAsync(email);
            if (retorno == null)
            {
                var user = new Usuario
                {
                    UserName = email,
                    Email = email,
                    Nome = name,
                    EmailConfirmed = true,
                    TennantId = tennantId != null ? tennantId : null
                };

                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, roles);
                }

                return result;
            }
            else
            {
                return default;
            }
        }

        private async Task<IdentityResult> CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new Funcao
                {
                    Name = roleName
                };
                return await _roleManager.CreateAsync(role);
            }
            return default;
        }

        private async Task CreateTennant()
        {
            var exists = await _context.Tennants.AnyAsync();
            if (exists != true)
            {
                var entity = new Tennant
                {
                    TennantId = TennantIDModel,
                    Status = TennantStatus.Ativo,
                    Nome = "Cursinho Gauss",
                    Logo = "b2ab9e91-fcfc-4811-aaca-35ad28d9d73c.png",
                    Endereco = "Avenida Nove de Julho, 288 - Centro Atibaia, SP 12940-580",
                    Email = "gauss@email.com",
                    Telefone = "(11) 4412-1234",
                    Whatsapp = "(11) 93012-2379"
                };
                _context.Tennants?.Add(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
