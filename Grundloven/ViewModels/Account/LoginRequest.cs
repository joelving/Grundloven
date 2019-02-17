using System.ComponentModel.DataAnnotations;

namespace Grundloven.ViewModels.Account
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Du skal angive en emailadresse.")]
        [EmailAddress(ErrorMessage = "Du skal angive en gyldig emailadresse.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du skal angive et kodeord.")]
        [StringLength(100, ErrorMessage = "Kodeordet skal være mindst {2} og højest {1} tegn langt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
