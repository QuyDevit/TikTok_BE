
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TiktokBackend.API.Filters
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public CustomAuthorizeAttribute(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if( context.ActionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute))
                return;
            var httpContext = context.HttpContext;
            if (!httpContext.Items.TryGetValue("UserId", out var userId) ||
                !httpContext.Items.TryGetValue("Role", out var role) ||
                userId == null || role == null || !string.Equals(role.ToString(), _role, StringComparison.OrdinalIgnoreCase))
            {

                context.Result = new UnauthorizedResult();
                return;
            }
            if (!string.Equals(role.ToString(), _role, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
