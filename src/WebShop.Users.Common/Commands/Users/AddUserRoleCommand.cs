using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebShop.Common.Validation;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class AddUserRoleCommand:ICommand
    {
        [NotEmptyGuid]
        [JsonIgnore]
        public Guid UserId { get; }

        [Required]
        public String RoleName { get; }

        [JsonConstructor]
        public AddUserRoleCommand(Guid userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

    }
}

