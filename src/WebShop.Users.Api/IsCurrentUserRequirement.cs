using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Users.Api
{
    public class IsCurrentUserRequirement: IAuthorizationRequirement
    {
        public Guid UserId { get; }
        public IsCurrentUserRequirement(Guid userId)
        {
            UserId = userId;
        }
    }
}
