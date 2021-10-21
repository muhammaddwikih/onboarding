using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace onboarding.api.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizedByRoleAttribute : AuthorizeAttribute
    {
        public AuthorizedByRoleAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}
