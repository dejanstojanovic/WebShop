using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
   public class AddRoleCommand:ICommand
    {
        public Guid RoleId { get;  }
        public String RoleName { get; }

        public AddRoleCommand(Guid roleId, String roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
    }
}
