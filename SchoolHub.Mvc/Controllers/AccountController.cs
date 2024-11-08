using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Entities.Users;
using SchoolHub.Mvc.Areas.AdminSchoolHub.Controllers;
using SchoolHub.Mvc.ViewModels;

namespace SchoolHub.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        public AccountController(ICombProvider comb, AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) : base(comb, context, userManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Where(t => t.TennantId == this._tennantIdUserLoggedIn).ToListAsync();
            return View(users);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}
