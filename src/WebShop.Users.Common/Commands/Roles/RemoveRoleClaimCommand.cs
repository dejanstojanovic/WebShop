using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
   public class RemoveRoleClaimCommand : ICommand
    {
        [Required]
        public String RoleName { get; }
        [Required]
        public String ClaimType { get;  }
        [Required]
        public String ClaimValue { get;  }

        [JsonConstructor]
        public RemoveRoleClaimCommand(String roleName, String claimType, String claimValue)
        {
            RoleName = roleName;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

    }
}
