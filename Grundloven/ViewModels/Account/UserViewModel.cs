using Grundloven.Models;
using NodaTime;

namespace Grundloven.ViewModels.Account
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public Instant? LockoutEnd { get; set; }

        public static UserViewModel FromUser(ApplicationUser user)
            => new UserViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnd = user.LockoutEnd.HasValue ? Instant.FromDateTimeOffset(user.LockoutEnd.Value) : (Instant?)null
            };
    }
}
