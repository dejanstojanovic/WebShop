using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
   public class AddRoleCommand:ICommand
    {
        public Guid UserId { get;  }
        public String RoleNAme { get; }

        public AddRoleCommand(Guid userId, String roleName)
        {
            UserId = userId;
        }
    }
}
