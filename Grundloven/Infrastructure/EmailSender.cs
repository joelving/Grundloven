using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Grundloven.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // TODO: Implement SendGrid or similar.
            return Task.CompletedTask;
        }
    }
}
