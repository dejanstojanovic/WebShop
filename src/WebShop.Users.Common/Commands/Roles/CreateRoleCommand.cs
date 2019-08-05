using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
   public class CreateRoleCommand:ICommand
    {
        public Guid Id { get;  }
        [Required]
        public String Name { get; }

        [JsonConstructor]
        public CreateRoleCommand(Guid roleId, String roleName)
        {
            Id = roleId==Guid.Empty ? Guid.NewGuid() : roleId;
            Name = roleName;
        }
    }
}
