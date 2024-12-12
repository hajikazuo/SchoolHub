using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SchoolHub.Common.Data;

namespace SchoolHub.Mvc.Areas.AdminSchoolHub.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICombProvider comb, AppDbContext context) : base(comb, context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
