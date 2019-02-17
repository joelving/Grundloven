using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grundloven.Infrastructure
{
    public class CustomValidationProblemDetails : ValidationProblemDetails
    {
        public CustomValidationProblemDetails(ModelStateDictionary modelState)
            : base()
        {
            Title = "Vi kunne ikke validere din forespørgsel.";
            Detail = "En eller flere dele af din forespøgsel var mangelfuld eller ikke korrekt.";
        }
    }
}
