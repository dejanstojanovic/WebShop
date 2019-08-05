using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Users.Common.Commands.Roles
{
    public class RemoveRoleCommand
    {
        public String RoleName { get; }

        [JsonConstructor]
        public RemoveRoleCommand(String roleName)
        {
            RoleName = roleName;
        }
    }
}
