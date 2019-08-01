using WebShop.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Users.AppServices.Commands
{
    public class ProfileImageRemoveCommand:ICommand
    {
        public Guid UserId { get;  }
        public ProfileImageRemoveCommand(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
