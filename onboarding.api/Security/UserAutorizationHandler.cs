﻿using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace onboarding.api.Security
{
    public class UserAutorizationHandler : AuthorizationHandler<Permission>
    {
        public UserAutorizationHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Permission permission)
        {
            IEnumerable<Claim> userRole = context.User.Claims.Where(c => c.Type == ClaimTypes.Role);

            if (userRole.Where(r => r.Value == permission.PermissionName).Any())
            {
                context.Succeed(permission);
            }
        }

        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            IEnumerable<string> userRole = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value);

            if (context.Requirements.First() is RolesAuthorizationRequirement roleReq)
            {
                IEnumerable<string> allowed = roleReq.AllowedRoles.Intersect(userRole);
                if (allowed.Any())
                {
                    context.Succeed(context.Requirements.First());
                }
            }

        }
    }
}
