using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class SetUserImageCommand:ICommand
    {
        public Guid UserId { get;  }
        public byte[] Image { get; }

        public SetUserImageCommand(Guid userId, byte[] image)
        {
            this.UserId = userId;
            this.Image = image;
        }
    }
}
