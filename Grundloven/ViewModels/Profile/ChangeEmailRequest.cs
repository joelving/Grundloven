using System;
using System.ComponentModel.DataAnnotations;

namespace Grundloven.ViewModels.Profile
{
    public class ChangeEmailRequest
    {
        [Required(ErrorMessage = "Du skal angive en emailadresse.")]
        [EmailAddress(ErrorMessage = "Du skal angive en gyldig emailadresse.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du skal angive en bekræftelseskode.")]
        public string Code { get; set; }
    }
}
