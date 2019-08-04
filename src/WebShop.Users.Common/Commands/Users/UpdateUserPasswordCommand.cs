using WebShop.Users.Common.Dtos.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Commands
{
    public class UpdateUserPasswordCommand: ICommand
    {
        public Guid UserId { get; }
        public String OldPassword { get; }
        public String NewPassword { get; }

        [JsonConstructor]
        public UpdateUserPasswordCommand(Guid userId, String oldPassword, String newPassword)
        {
            this.UserId = userId;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }

        public UpdateUserPasswordCommand(Guid userId,UserPasswordUpdateDto passwordUpdate)
        {
            this.UserId = userId;
            this.OldPassword = passwordUpdate.OldPassword;
            this.NewPassword = passwordUpdate.NewPassword;
        }
    }
}

