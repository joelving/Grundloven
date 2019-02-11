using Grundloven.Data;
using Grundloven.Infrastructure;
using Grundloven.Models;
using Grundloven.ViewModels.Profile;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Grundloven.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<SendVerificationEmailResponse> SendVerificationEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new SendVerificationEmailResponse
                {
                    Success = false,
                    ErrorMessages = new List<string> { "Kunne ikke genkende din bruger. Prøv at logge ud og ind igen." }
                };
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/account/confirmemail",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);
            var (subject, body) = EmailTemplates.ConfirmEmail(callbackUrl);
            await _emailSender.SendEmailAsync(email, subject, body);

            return new SendVerificationEmailResponse
            {
                Success = true
            };
        }
    }
}
