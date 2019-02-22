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

        [HttpPost("/api/profile/initiate-email-change")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        public async Task<ActionResult> InitiateEmailChange([FromBody]InitiateEmailChangeRequest model)
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
                values: new { email = model.Email, code },
                protocol: Request.Scheme);
            var (subject, body) = EmailTemplates.ChangeEmail(user.UserName, callbackUrl, code);
            await _emailSender.SendEmailAsync(model.Email, subject, body);

            return Ok();
        }

        [HttpGet("/profile/change-email")]
        [Produces("application/json")]
        [ProducesResponseType(302)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        public async Task<ActionResult> ChangeEmail(ChangeEmailRequest model)
            => await ChangeEmailInternal(model, () => Redirect("/profile/me"));

        [HttpPost("/api/profile/change-email")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        public async Task<ActionResult> ApiChangeEmail([FromBody]ChangeEmailRequest model)
            => await ChangeEmailInternal(model, () => Ok());

        private async Task<ActionResult> ChangeEmailInternal(ChangeEmailRequest model, Func<ActionResult> successGenerator)
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

            var result = await _userManager.ChangeEmailAsync(user, model.Email, model.Code);
            if (!result.Succeeded)
            {
                return Unauthorized(new IdentityProblemDetails(result.Errors));
            }

            return successGenerator();
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
