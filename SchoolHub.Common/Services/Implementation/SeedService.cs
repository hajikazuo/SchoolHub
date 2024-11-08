using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities;
using SchoolHub.Common.Models.Entities.Enum;
using SchoolHub.Common.Models.Entities.Users;
using SchoolHub.Common.Services.Interfaces;

namespace SchoolHub.Common.Services.Implementation
{
    public class SeedService : ISeedService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _context;

        private const string AdminRole = "Admin";
        private const string UserRole = "User";

        private Guid TennantIDModel = new Guid("3b7b2c08-601d-46c9-b047-32f8c601649b");

        public SeedService(UserManager<User> userManager, RoleManager<Role> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Seed()
        {
            CreateTennant().GetAwaiter().GetResult();
            CreateRole(AdminRole).GetAwaiter().GetResult();
            CreateRole(UserRole).GetAwaiter().GetResult();
            CreateUser("admin@escola.com.br", name: "Admin User", "Teste@2024", role: AdminRole, TennantIDModel).GetAwaiter().GetResult();     
        }

        private async Task<IdentityResult> CreateUser(string email, string name, string password, string role, Guid? tennantId = null)
        {
            var retorno = await _userManager.FindByEmailAsync(email);
            if (retorno == null)
            {
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    Name = name,
                    EmailConfirmed = true,
                    TennantId = tennantId != null ? tennantId : null
                };

                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, role).Wait();
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
                var role = new Role
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
                    Status = TennantStatus.Active,
                    Name = "Cursinho Gauss"
                };
                _context.Tennants?.Add(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
