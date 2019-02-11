using Grundloven.Data;
using Grundloven.Infrastructure;
using Grundloven.Models;
using Grundloven.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Grundloven.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;

        public AccountController(
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
        public async Task<RegisterResponse> Register([FromBody]RegisterRequest input)
        {
            if (!ModelState.IsValid)
            {
                return new RegisterResponse
                {
                    Success = false,
                    ErrorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList()
                };
            }


            var user = new ApplicationUser { UserName = input.Email, Email = input.Email };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/account/confirm-email",
                    pageHandler: null,
                    values: new { userId = user.Id, code },
                    protocol: Request.Scheme);

                var (subject, body) = EmailTemplates.ConfirmEmail(callbackUrl);
                await _emailSender.SendEmailAsync(input.Email, subject, body);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return new RegisterResponse
                {
                    Success = true,
                    Profile = UserViewModel.FromUser(user)
                };
            }

            return new RegisterResponse
            {
                Success = false,
                ErrorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList()
            };
        }

        [HttpPost]
        public async Task<LoginResponse> Login([FromBody]LoginRequest input)
        {
            if (!ModelState.IsValid)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList()
                };
            }
            
            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return new LoginResponse
                {
                    Success = true,
                    Profile = UserViewModel.FromUser(await _userManager.FindByEmailAsync(input.Email))
                };
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList()
                };
            }
        }

        [HttpPost]
        public async Task<LogoutResponse> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return new LogoutResponse { Success = true };
        }

        [HttpPost]
        public async Task<ForgotPasswordResponse> ForgotPassword([FromBody]ForgotPasswordRequest input)
        {
            if (!ModelState.IsValid)
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    ErrorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList()
                };
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return new ForgotPasswordResponse
                {
                    Success = false
                };
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "/account/resetpassword",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);

            var (subject, body) = EmailTemplates.ResetPassword(callbackUrl);
            await _emailSender.SendEmailAsync(input.Email, subject, body);
            
            return new ForgotPasswordResponse
            {
                Success = true
            };
        }

        [HttpPost]
        public async Task<ResetPasswordResponse> ResetPassword([FromBody]ResetPasswordRequest input)
        {
            if (!ModelState.IsValid)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    ErrorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList()
                };
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return new ResetPasswordResponse
                {
                    Success = false
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, input.Code, input.Password);
            if (result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = true
                };
            }
            
            return new ResetPasswordResponse
            {
                Success = false,
                ErrorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList()
            };
        }
    }
}
