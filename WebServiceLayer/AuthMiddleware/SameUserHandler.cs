using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebServiceLayer.Auth_Middleware
{
    public class SameUserHandler : AuthorizationHandler<SameUserRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SameUserHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameUserRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return Task.CompletedTask;

            var routeUserId = httpContext.GetRouteValue("userId")?.ToString();
            var jwtUserId = context.User.FindFirst("id")?.Value
                            ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(jwtUserId)
                && !string.IsNullOrEmpty(routeUserId)
                && string.Equals(routeUserId, jwtUserId, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
