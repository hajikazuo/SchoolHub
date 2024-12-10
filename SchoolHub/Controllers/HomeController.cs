using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Usuarios;

namespace SchoolHub.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICombProvider comb, AppDbContext context, UserManager<Usuario> userManager) : base(comb, context, userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
