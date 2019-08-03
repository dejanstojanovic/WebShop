using WebShop.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Users.Common.Commands
{
    public class RemoveUserImageCommand:ICommand
    {
        public Guid UserId { get;  }
        public RemoveUserImageCommand(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
