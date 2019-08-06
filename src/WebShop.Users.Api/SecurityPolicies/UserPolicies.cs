using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Users.Api.SecurityPolicies
{
    public static class UserPolicies
    {
        public const String UserCreatePolicy = "UserCreatePolicy";
        public const String UserModifyPolicy = "UserModifyPolicy";
        public const String UserViewPolicy = "UserViewPolicy";
        public const String SameUserPolicy = "SameUserPolicy";
        public const String UserAdminPolicy = "Admin";
    }
}
