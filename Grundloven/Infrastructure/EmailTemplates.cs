using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grundloven.Infrastructure
{
    public static class EmailTemplates
    {
        public static (string subject, string body) ConfirmEmail(string callbackUrl)
            => (
                "Bekræft din email - Grundloven",
                "<p><strong>Velkommen til Grundlov.nu.</strong></p>" +
                $"<p>Bekræft venligst din email adresse ved at gå til <a href=\"{callbackUrl}\" title=\"Bekræft din email adresse\">{callbackUrl}</a>.</p>" +
                "<p>Vi glæder os til at se, hvad du finder på!</p>" +
                "<p>Mange hilsener,<br />Grundlov.nu-teamet</p>"
            );

        public static (string subject, string body) ResetPassword(string callbackUrl)
            => (
                "Nulstil kodeord - Grundloven",
                "<p>Du modtager denne mail, fordi nogen har bedt om at få nulstillet kodeordet til din bruger på Grundlov.nu. Hvis det ikke var dig, kan du roligt ignorere denne mail - intet ændrer sig.</p>" +
                $"<p>For at nulstille din kode, skal du gå til <a href=\"{callbackUrl}\" title=\"Nulstil kodeord\">{callbackUrl}</a> og vælge en ny.</p>" +
                "<p>Mange hilsener,<br />Grundlov.nu-teamet</p>"
            );

        public static (string subject, string body) RegisterExistingEmail(string username, string callbackUrl)
            => (
                "Nulstil kodeord - Grundloven",
                $"<p>Kære {username},</p>" +
                "<p>Du modtager denne mail, fordi en bruger er forsøgt oprettet med din email på Grundlov.nu, hvor der allerede findes en bruger med din mail tilknyttet.</p>" +
                "<p>Hvis det ikke var dig, kan du roligt ignorere denne mail - intet ændrer sig.</p>" +
                $"<p>For at nulstille din kode, skal du gå til <a href=\"{callbackUrl}\" title=\"Nulstil kodeord\">{callbackUrl}</a> og vælge en ny.</p>" +
                "<p>Mange hilsener,<br />Grundlov.nu-teamet</p>"
            );

        public static (string subject, string body) ChangeEmail(string username, string callbackUrl)
            => (
                "Nulstil kodeord - Grundloven",
                $"<p>Kære {username},</p>" +
                "<p>Du modtager denne mail, fordi en bruger er forsøgt oprettet med din email på Grundlov.nu, hvor der allerede findes en bruger med din mail tilknyttet.</p>" +
                "<p>Hvis det ikke var dig, kan du roligt ignorere denne mail - intet ændrer sig.</p>" +
                $"<p>For at nulstille din kode, skal du gå til <a href=\"{callbackUrl}\" title=\"Nulstil kodeord\">{callbackUrl}</a> og vælge en ny.</p>" +
                "<p>Mange hilsener,<br />Grundlov.nu-teamet</p>"
            );
    }
}
