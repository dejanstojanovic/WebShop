using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class RemoveRoleCommand:ICommand
    {
        [JsonIgnore]
        public String RoleName { get; }

        [JsonConstructor]
        public RemoveRoleCommand(String roleName)
        {
            RoleName = roleName;
        }
    }
}
