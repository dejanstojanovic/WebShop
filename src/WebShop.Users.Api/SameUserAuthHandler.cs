using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace WebShop.Users.Api
{
    public class SameUserAuthHandler : AuthorizationHandler<SameUserAuthReqirement>
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        public SameUserAuthHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserAuthReqirement requirement)
        {

            var isSameUser = Guid.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value, out var claimUserId) &&
                Guid.TryParse(_httpContextAccessor.HttpContext.GetRouteValue("userId") as String, out var routeUserId) &&
                claimUserId != Guid.Empty & claimUserId == routeUserId;

            var isAdmin = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Any(c => c.Value.Equals("admin", StringComparison.InvariantCultureIgnoreCase));

            if (isSameUser || isAdmin)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
