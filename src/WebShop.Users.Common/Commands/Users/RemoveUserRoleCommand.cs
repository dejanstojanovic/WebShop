using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class RemoveUserRoleCommand:ICommand
    {
        public Guid UserId { get; }
        public String RoleName { get; }

        public RemoveUserRoleCommand(Guid userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

    }
}

