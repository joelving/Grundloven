using Microsoft.AspNetCore.Mvc;

namespace Grundloven.ViewModels.Account
{
    public class ResetPasswordProblem : ProblemDetails
    {
        public ResetPasswordProblem()
        {
            this.Title = "Der opstod et problem.";
            this.Detail = "Vi kunne desværre ikke nulstille dit kodeord. Af sikkerhedshensyn kan vi ikke fortælle mere. Prøv at nulstille dit kodeord påny.";
        }
    }
}
