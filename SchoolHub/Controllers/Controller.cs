using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Usuarios;

namespace SchoolHub.Mvc.Controllers
{
    [Authorize]
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        protected readonly ICombProvider _comb;
        protected readonly AppDbContext _context;
        protected readonly UserManager<Usuario> _userManager;

        public Guid _tennantIdUsuarioLogado { get; set; }
        public Guid _turmaUsuarioLogado { get; set; }

        public Controller(ICombProvider comb, AppDbContext context, UserManager<Usuario> userManager)
        {
            _comb = comb;
            _context = context;
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity.IsAuthenticated)
            {
                var thisUser = _userManager.FindByNameAsync(this.User.Identity.Name).GetAwaiter().GetResult();
                _tennantIdUsuarioLogado = thisUser.TennantId ?? Guid.Empty;
                _turmaUsuarioLogado = thisUser.TurmaId ?? Guid.Empty;

                var logo = _context.Tennants.Find(_tennantIdUsuarioLogado)?.Logo;
                ViewBag.Logo = $"/assets/img/logo/{logo}";
            }
        }
    }
}
