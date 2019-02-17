using Microsoft.AspNetCore.Identity;

namespace Grundloven.Infrastructure
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string username)
            => new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"Brugernavnet '{username}' er allerede i brug."
            };

        public override IdentityError InvalidUserName(string username)
            => new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"Brugernavnet '{username}' er ikke gyldigt."
            };

        public override IdentityError DuplicateEmail(string email)
            => new IdentityError {
                Code = nameof(DuplicateEmail),
                Description = $"Emailadressen '{email}' er allerede i brug."
            };

        public override IdentityError InvalidEmail(string email)
            => new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = $"Emailadressen '{email}' er ikke gyldig."
            };

        public override IdentityError InvalidToken()
            => new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "Den angivne token er ikke gyldig."
            };

        public override IdentityError PasswordRequiresDigit()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Kodeord skal indeholde tal."
            };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "Kodeord skal indeholde små bogstaver."
            };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Kodeord skal indeholde andre tegn end tal og bogstaver (eks. !#_)."
            };

        public override IdentityError PasswordRequiresUniqueChars(int count)
            => new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = $"Kodeord skal indeholde minimum {count} forskellige tegn."
            };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Kodeord skal indeholde versaler (store bogstaver)."
            };

        public override IdentityError PasswordTooShort(int minLength)
            => new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Kodeordet er for kort. Kodeord skal være minimum {minLength} tegn langt."
            };

        public override IdentityError UserAlreadyHasPassword()
            => new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = "Brugeren har allerede et kodeord. Kodeordet skal derfor sættes via 'ændre kodeord'-funktionen."
            };

        public override IdentityError DefaultError()
            => new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "Der opstod en ikke nærmere defineret fejl. Prøv igen senere eller kontakt en administrator."
            };

        public override IdentityError PasswordMismatch()
            => new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "Kodeordet er forkert."
            };

        public override IdentityError ConcurrencyFailure()
            => new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = "Samtidighedsproblem. Objektet er blevet ændret."
            };

        public override IdentityError DuplicateRoleName(string role)
            => new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = $"Rollen '{role}' findes allerede."
            };

        public override IdentityError InvalidRoleName(string role)
            => new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = $"Rollenavnet '{role}' er ugyldigt."
            };

        public override IdentityError LoginAlreadyAssociated()
            => new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = "En bruger med dette login findes allerede."
            };

        public override IdentityError RecoveryCodeRedemptionFailed()
            => new IdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = "Gendannelseskoden kunne ikke indløses."
            };

        public override IdentityError UserAlreadyInRole(string role)
            => new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = $"Brugeren er allerede i rollen '{role}'."
            };

        public override IdentityError UserLockoutNotEnabled()
            => new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = "Lockout er ikke slået til for denne bruger."
            };

        public override IdentityError UserNotInRole(string role)
            => new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = $"Brugeren er ikke i rollen '{role}'."
            };
    }
}
