using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RT.Comb;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Enums;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Interface;
using SchoolHub.Mvc.Extensions;
using SchoolHub.Mvc.Services.Interface;
using SchoolHub.Mvc.ViewModels.AccountViewModels;

namespace SchoolHub.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUploadService _uploadService;
        public AccountController(IUsuarioRepository usuarioRepository, IUploadService uploadService, ICombProvider comb, AppDbContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager) : base(comb, context, userManager)
        {
            _signInManager = signInManager;
            _usuarioRepository = usuarioRepository;
            _uploadService = uploadService;
        }

        public async Task<IActionResult> Index()
        {
            var tennantid = this._tennantIdUsuarioLogado;
            var usuarios = await _usuarioRepository.GetAllAsync(tennantid);
            return View(usuarios);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioRepository.GetByIdAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["Funcao"] = (await _userManager.GetRolesAsync(usuario)).FirstOrDefault();
            return View(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Funcoes"] = new SelectList(await _usuarioRepository.GetAllRolesAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model, IFormFile? file, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    TennantId = this._tennantIdUsuarioLogado,
                    UserName = model.UserName,
                    Nome = model.Nome,
                    Email = model.UserName,
                    Celular = model.Celular,
                    Imagem = await _uploadService.UploadFoto(file, PastaUpload.FotoUsuario),
                    Documentos = model.Documentos
                };

                var result = await _usuarioRepository.CreateAsync(usuario, model.Password);

                if (result.Succeeded)
                {

                    if (model.SelectedRole != null)
                    {
                        var funcoesUsuario = await _userManager.GetRolesAsync(usuario);
                        await _userManager.RemoveFromRolesAsync(usuario, funcoesUsuario);
                        await _userManager.AddToRoleAsync(usuario, model.SelectedRole);
                    }

                    return RedirectToAction(nameof(Index));
                }
                AddErrors(result);
            }

            ViewData["Funcoes"] = new SelectList(await _usuarioRepository.GetAllRolesAsync(), "Id", "Nome");
            ViewData["Tipo"] = this.MontarSelectListParaEnum(new TipoDocumento());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, string returnUrl = null)
        {
            var usuarioDb = await _usuarioRepository.GetByIdAsync(id);
            if (usuarioDb == null)
            {
                return NotFound();
            }

            var funcao = await _userManager.GetRolesAsync(usuarioDb);

            var model = new RegisterViewModel
            {
                Id = usuarioDb.Id,
                UserName = usuarioDb.UserName ?? String.Empty,
                Nome = usuarioDb.Nome,
                Celular = usuarioDb.Celular,
                Imagem = usuarioDb.Imagem,
                SelectedRole = funcao.FirstOrDefault(),
                Documentos = usuarioDb.Documentos
            };

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Funcoes"] = new SelectList(await _usuarioRepository.GetAllRolesAsync(), "Id", "Nome");
            ViewData["Tipo"] = this.MontarSelectListParaEnum(new TipoDocumento());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterViewModel model, Guid id, IFormFile? file, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                usuario.UserName = model.UserName;
                usuario.Nome = model.Nome;
                usuario.Email = model.UserName;
                usuario.Celular = model.Celular;
                usuario.Documentos = model.Documentos;

                if (file != null)
                {
                    usuario.Imagem = await _uploadService.UploadFoto(file, PastaUpload.FotoUsuario);
                }
                else
                {
                    usuario.Imagem = model.Imagem;
                }

                var result = await _usuarioRepository.UpdateAsync(usuario);

                if (result.Succeeded)
                {
                    if (model.SelectedRole != null)
                    {
                        var funcoesUsuario = await _userManager.GetRolesAsync(usuario);
                        await _userManager.RemoveFromRolesAsync(usuario, funcoesUsuario);
                        await _userManager.AddToRoleAsync(usuario, model.SelectedRole);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Funcoes"] = new SelectList(await _usuarioRepository.GetAllRolesAsync(), "Id", "Nome");
            ViewData["Tipo"] = this.MontarSelectListParaEnum(new TipoDocumento());
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);

            if (usuario != null)
            {
                var result = await _usuarioRepository.DeleteAsync(usuario);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(Guid id)
        {
            var usuarioDb = await _usuarioRepository.GetByIdAsync(id);
            if (usuarioDb == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel
            {
                Id = usuarioDb.Id,
            };

            ViewBag.Confirm = TempData["Confirm"];
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _usuarioRepository.GetByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    AddErrors(removePasswordResult);
                    return View(model);
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                {
                    AddErrors(addPasswordResult);
                    return View(model);
                }
            }

            var updateResult = await _usuarioRepository.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            AddErrors(updateResult);

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> NewDocumento()
        {
            ViewData["Tipo"] = this.MontarSelectListParaEnum(new TipoDocumento());
            return PartialView("Documentos", new Documento());
        }

        [HttpGet]
        public IActionResult CadastrarEmMassa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarEmMassa(IFormFile arquivoExcel)
        {
            if (arquivoExcel == null || arquivoExcel.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var usuarios = await _uploadService.ProcessarExcel(arquivoExcel);

            if (usuarios != null)
            {
                foreach (var usuario in usuarios)
                {
                    usuario.Id = _comb.Create();
                    usuario.TennantId = this._tennantIdUsuarioLogado;
                    var result = await _userManager.CreateAsync(usuario, "SenhaPadrao123!");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(usuario, "Aluno");
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
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
