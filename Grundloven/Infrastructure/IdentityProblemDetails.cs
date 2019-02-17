using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Grundloven.Infrastructure
{
    public class IdentityProblemDetails : ProblemDetails
    {
        public IdentityProblemDetails(IEnumerable<IdentityError> errors)
        {
            Errors = errors.Where(e => !IsSensitiveError(e));
        }

        private bool IsSensitiveError(IdentityError error)
            => error.Code == nameof(CustomIdentityErrorDescriber.DuplicateEmail)
            || error.Code == nameof(CustomIdentityErrorDescriber.DuplicateUserName);
        
        public IEnumerable<IdentityError> Errors { get; }
    }
}
