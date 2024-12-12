using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Mvc.ViewModels.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        public Guid Id { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirmar senha")]
        public string ConfirmPassword { get; set; }
    }
}
