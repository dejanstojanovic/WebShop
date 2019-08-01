using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Services.Commands
{
    public class ProfileImageSetCommand:ICommand
    {
        public Guid UserId { get;  }
        public byte[] Image { get; }

        public ProfileImageSetCommand(Guid userId, byte[] image)
        {
            this.UserId = userId;
            this.Image = image;
        }
    }
}
