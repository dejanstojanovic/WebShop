using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
   public class RemoveRoleCommand : ICommand
    {
        public String RoleName { get; }

        public RemoveRoleCommand(String roleName)
        {
            RoleName = roleName;
        }
    }
}
