using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SchoolHub.Common.Data;

namespace SchoolHub.Mvc.Areas.AdminSchoolHub.Controllers
{
    [Area("AdminSchoolHub")]
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        protected readonly ICombProvider _comb;
        protected readonly AppDbContext _context;

        public Controller(ICombProvider comb, AppDbContext context)
        {
            _comb = comb;
            _context = context;
        }
    }
}
