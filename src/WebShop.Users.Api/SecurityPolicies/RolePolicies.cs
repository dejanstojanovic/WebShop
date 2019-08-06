using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Users.Api.SecurityPolicies
{
    public static class RolePolicies
    {
        public const String RoleCreatePolicy = "RoleCreatePolicy";
        public const String RoleModifyPolicy = "RoleModifyPolicy";
        public const String RoleViewPolicy = "RoleViewPolicy";
        public const String RoleAdminPolicy = "Admin";
    }
}
