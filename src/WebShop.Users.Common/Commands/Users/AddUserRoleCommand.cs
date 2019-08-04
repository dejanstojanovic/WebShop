using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class AddUserRoleCommand:ICommand
    {
        public Guid UserId { get; }
        public String RoleName { get; }

        public AddUserRoleCommand(Guid userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

    }
}

