using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities.Users;

namespace SchoolHub.Mvc.Controllers
{
    [Authorize]
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        protected readonly ICombProvider _comb;
        protected readonly AppDbContext _context;
        protected readonly UserManager<User> _userManager;

        public Guid _tennantIdUserLoggedIn { get; set; }

        public Controller(ICombProvider comb, AppDbContext context, UserManager<User> userManager)
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
                _tennantIdUserLoggedIn = thisUser.TennantId ?? Guid.Empty;
            }
        }
    }
}
