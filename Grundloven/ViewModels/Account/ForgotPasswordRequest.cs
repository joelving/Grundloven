using System.ComponentModel.DataAnnotations;

namespace Grundloven.ViewModels.Account
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Du skal angive en emailadresse.")]
        [EmailAddress(ErrorMessage = "Du skal angive en gyldig emailadresse.")]
        public string Email { get; set; }
    }
}
