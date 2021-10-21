using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace onboarding.api.Security
{
    public class Permission : IAuthorizationRequirement
    {
        public Permission(string permissionName)
        {
            PermissionName = permissionName;
        }
        public string PermissionName { get; set; }
    }
}
