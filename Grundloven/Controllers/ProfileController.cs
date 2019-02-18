using Grundloven.Data;
using Grundloven.Infrastructure;
using Grundloven.Models;
using Grundloven.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Grundloven.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;

        public ProfileController(
            ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender
            )
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost("/api/profile/request-email-change")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        public async Task<ActionResult> RequestEmailChange([FromBody]ChangeEmailRequest model)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(new CustomValidationProblemDetails(ModelState));

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Could not find user. Få til at logge ind igen.
                return Unauthorized(new ProblemDetails
                {
                    Title = "Login udløbet",
                    Detail = "Vi kunne ikke genkende din bruger. Prøv at logge ud og ind igen."
                });
            }

            // Validate password to prevent hijacking already signed in users.
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Forkert kodeord",
                    Detail = "Kodeordet er ikke korrekt. Prøv igen. Ved gentagne forkerte forsøg låses din konto."
                });
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
            var callbackUrl = Url.Page(
                "/profile/change-email",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);
            var (subject, body) = EmailTemplates.ChangeEmail(user.UserName, callbackUrl);
            await _emailSender.SendEmailAsync(model.Email, subject, body);

            return Ok();
        }

        [HttpGet("/profile/change-email")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        public async Task<ActionResult> ChangeEmail(Guid userId, string email, string code)
        {
            if (userId == default)
                ModelState.AddModelError(nameof(userId), "Du skal angive et bruger Id.");
            if (string.IsNullOrWhiteSpace(email))
                ModelState.AddModelError(nameof(email), "Du skal angive en email.");
            if (string.IsNullOrWhiteSpace(code))
                ModelState.AddModelError(nameof(code), "Du skal angive en bekræftelseskode.");
            if (!ModelState.IsValid)
                return UnprocessableEntity(new CustomValidationProblemDetails(ModelState));

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Could not find user. Få til at logge ind igen.
                return Unauthorized(new ProblemDetails
                {
                    Title = "Login udløbet",
                    Detail = "Vi kunne ikke genkende din bruger. Prøv at logge ud og ind igen."
                });
            }
            if (user.Id != userId)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Ikke tilladt",
                    Detail = "Du har forsøgt at ændre emailadressen for en bruger, der ikke er dig. Log ud og dernæst ind igen med den bruger, du forsøger at skifte koden på."
                });
            }

            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                return Unauthorized(new IdentityProblemDetails(result.Errors));
            }

            return Redirect("/profile/me");
        }

        [HttpPost("/api/profile/send-verification-email")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> SendVerificationEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Could not find user. Få til at logge ind igen.
                return Unauthorized(new ProblemDetails
                {
                    Title = "Login udløbet",
                    Detail = "Vi kunne ikke genkende din bruger. Prøv at logge ud og ind igen."
                });
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/account/confirm-email",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);
            var (subject, body) = EmailTemplates.ConfirmEmail(callbackUrl);
            await _emailSender.SendEmailAsync(email, subject, body);

            return Ok();
        }
    }
}
