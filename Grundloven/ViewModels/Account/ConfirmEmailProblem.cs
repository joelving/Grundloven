using Microsoft.AspNetCore.Mvc;

namespace Grundloven.ViewModels.Account
{
    public class ConfirmEmailProblem : ProblemDetails
    {
        public ConfirmEmailProblem()
        {
            this.Title = "Der opstod et problem.";
            this.Detail = "Vi kunne desværre ikke verificere din emailadresse med det angivne kodeord. Kontrollér koden for stavefejl og prøv igen. Hvis det fortsat ikke virker, kan du forsøge dig med siden 'glemt kodeord'-funktion.";
        }
    }
}
