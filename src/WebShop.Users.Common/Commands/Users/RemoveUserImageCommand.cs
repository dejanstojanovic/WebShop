using WebShop.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WebShop.Users.Common.Commands
{
    public class RemoveUserImageCommand:ICommand
    {
        public Guid UserId { get;  }
        [JsonConstructor]
        public RemoveUserImageCommand(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
