using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WebShop.Messaging;
using WebShop.Messaging;

namespace WebShop.Users.Tests.Controllers.v1
{
   public abstract class BaseControllerTests
    {
        protected void SetAuthenticationContext(ControllerBase controller)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(
                     type: "userid",
                     value: Guid.NewGuid().ToString())
            }));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}
