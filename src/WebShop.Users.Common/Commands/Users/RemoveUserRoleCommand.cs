using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebShop.Common.Validation;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class RemoveUserRoleCommand:ICommand
    {
        [NotEmptyGuid]
        public Guid UserId { get; }
        [Required]
        public String RoleName { get; }

        public RemoveUserRoleCommand(Guid userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

    }
}

