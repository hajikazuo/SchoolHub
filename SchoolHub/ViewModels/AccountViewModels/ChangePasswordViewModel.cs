using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Mvc.ViewModels.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        public Guid Id { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
