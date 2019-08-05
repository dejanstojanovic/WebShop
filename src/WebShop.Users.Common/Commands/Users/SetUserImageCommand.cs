using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebShop.Common.Validation;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class SetUserImageCommand:ICommand
    {
        [NotEmptyGuid]
        [Required]
        public Guid UserId { get;  }

        [Required]
        public byte[] Image { get; }

        public SetUserImageCommand(Guid userId, byte[] image)
        {
            this.UserId = userId;
            this.Image = image;
        }
    }
}
