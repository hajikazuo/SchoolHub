using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities.Users;
using SchoolHub.Mvc.Models;
using System.Diagnostics;

namespace SchoolHub.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICombProvider comb, AppDbContext context, UserManager<User> userManager) : base(comb, context, userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
