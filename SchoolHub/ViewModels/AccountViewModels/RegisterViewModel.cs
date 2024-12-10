using SchoolHub.Common.Models.Usuarios;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Mvc.ViewModels.AccountViewModels
{
    public class RegisterViewModel : Usuario
    {
        [Required]
        [Display(Name = "Login")]
        [EmailAddress(ErrorMessage = "Informe um endereço de email válido.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação devem ser iguais.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Selecione uma função.")]
        [Display(Name = "Função")]
        public string SelectedRole { get; set; }
    }
}
