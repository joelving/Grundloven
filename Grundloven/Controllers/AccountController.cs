using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Grundloven.Data;
using Grundloven.Infrastructure;
using Grundloven.Models;
using Grundloven.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Mvc.Internal;
using OpenIddict.Server;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IOptionsSnapshot<IdentityOptions> _identityOptions;

        public AccountController(
            ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender,
            IOptionsSnapshot<IdentityOptions> identityOptions
            )
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _identityOptions = identityOptions;
        }

        [HttpPost("/api/account/register")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody]RegisterRequest input)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(new CustomValidationProblemDetails(ModelState));
            
            var user = new ApplicationUser { UserName = input.Email, Email = input.Email };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                if (false && result.Errors.Any(e => e.Code == new CustomIdentityErrorDescriber().DuplicateUserName(user.UserName).Code))
                {
                    // Someone is trying to register with an occupied username. To prevent leaking info,
                    // we'll pretend all is well and send an email to the user in question with a link to
                    // reset in case he legitemately has forgotten.
                    return Created("/api/profile/me", new RegisterResponse());
                }
                else
                {
                    return BadRequest(new IdentityProblemDetails(result.Errors) {
                        Title = "Fejl i oprettelse",
                        Detail = "Der skete en fejl i oprettelsen af din bruger."
                    });
                }
            }

            _logger.LogInformation("User created a new account with password.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/account/confirm-email",
                pageHandler: null,
                values: new { userId = user.Id, code },
                protocol: Request.Scheme);

            var (subject, body) = EmailTemplates.ConfirmEmail(callbackUrl);
            //await _emailSender.SendEmailAsync(input.Email, subject, body);
            
            return Created("/api/profile/me", new RegisterResponse());
        }

        [HttpPost("/api/account/login")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<OpenIdConnectResponse>> Login([ModelBinder(typeof(OpenIddictMvcBinder))][FromForm] OpenIdConnectRequest request)
        {
            if (request.IsPasswordGrantType())
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "Den angivne login-type understøttes ikke."
                });
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "Ugyldigt login."
                });
            }

            // Validate the username/password parameters and ensure the account is not locked out.
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Unauthorized(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "Ugyldigt login."
                });
            }

            // Create a new authentication ticket.
            var ticket = await CreateTicketAsync(request, user);

            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);

        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, ApplicationUser user)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), OpenIddictServerDefaults.AuthenticationScheme);

            // Set the list of scopes granted to the client application.
            ticket.SetScopes(new[]
            {
                OpenIdConnectConstants.Scopes.OpenId,
                OpenIdConnectConstants.Scopes.Email,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes()));

            ticket.SetResources("resource-server");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                var destinations = new List<string>
                {
                    OpenIdConnectConstants.Destinations.AccessToken
                };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            return ticket;
        }

        [HttpPost("/api/account/logout")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok();
        }

        [HttpPost("/api/account/forgot-password")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        public async Task<ActionResult> ForgotPassword([FromBody]ForgotPasswordRequest input)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(new CustomValidationProblemDetails(ModelState));

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok();
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "/account/reset-password",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);

            var (subject, body) = EmailTemplates.ResetPassword(callbackUrl);
            await _emailSender.SendEmailAsync(input.Email, subject, body);

            return Ok();
        }

        [HttpPost("/api/account/reset-password")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<ActionResult<ResetPasswordProblem>> ResetPassword([FromBody]ResetPasswordRequest input)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(new CustomValidationProblemDetails(ModelState));

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest(new ResetPasswordProblem());
            }

            var result = await _userManager.ResetPasswordAsync(user, input.Code, input.Password);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(new ResetPasswordProblem());
        }
    }
}
