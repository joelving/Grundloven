using System.ComponentModel.DataAnnotations;

namespace Grundloven.ViewModels.Account
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Du skal angive en emailadresse.")]
        [EmailAddress(ErrorMessage = "Du skal angive en gyldig emailadresse.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du skal angive et kodeord.")]
        [StringLength(100, ErrorMessage = "Kodeordet skal være mindst {2} og højest {1} tegn langt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
