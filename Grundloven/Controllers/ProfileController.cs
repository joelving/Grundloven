using Grundloven.Data;
using Grundloven.Infrastructure;
using Grundloven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpPost("/api/account/send-verification-email")]
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
