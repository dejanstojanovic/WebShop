using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
   public class RemoveRoleClaimCommand : ICommand
    {
        public String RoleName { get; }
        public String ClaimType { get;  }
        public String ClaimValue { get;  }

        public RemoveRoleClaimCommand(String roleName, String claimType, String claimValue)
        {
            RoleName = roleName;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

    }
}
