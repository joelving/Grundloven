using System;
using System.ComponentModel.DataAnnotations;

namespace Grundloven.ViewModels.Account
{
    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = "Du skal angive et bruger Id.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Du skal angive en bekræftelseskode.")]
        public string Code { get; set; }
    }
}
