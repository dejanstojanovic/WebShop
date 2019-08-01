using WebShop.Users.Contracts.ApplicationUser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.AppServices.Commands
{
    public class UpdatePasswordCommand: ICommand
    {
        public Guid Id { get; }
        public String OldPassword { get; }
        public String NewPassword { get; }

        [JsonConstructor]
        public UpdatePasswordCommand(Guid id, String oldPassword, String newPassword)
        {
            this.Id = id;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }

        public UpdatePasswordCommand(Guid id,PasswordUpdate passwordUpdate)
        {
            this.Id = id;
            this.OldPassword = passwordUpdate.OldPassword;
            this.NewPassword = passwordUpdate.NewPassword;
        }
    }
}

