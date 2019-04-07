using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Starter.Net.Api.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequirePermission : Attribute, IAuthorizeData, IAuthorizationFilter
    {
        private string Permission { set; get; }
        public RequirePermission(string permission)
        {
            Permission = permission;
        }

        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionClaims = context.HttpContext.User.Claims.Where(c => c.Type == CustomClaims.Permission);
            var hasClaim = permissionClaims.Any(x => x.Value == Permission);
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
