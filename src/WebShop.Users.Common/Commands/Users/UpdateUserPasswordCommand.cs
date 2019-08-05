using WebShop.Users.Common.Dtos.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;
using WebShop.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Users.Common.Commands
{
    public class UpdateUserPasswordCommand: ICommand
    {
        [NotEmptyGuid]
        public Guid UserId { get; }
        [Required]
        public String OldPassword { get; }
        [Required]
        public String NewPassword { get; }

        [JsonConstructor]
        public UpdateUserPasswordCommand(Guid userId, String oldPassword, String newPassword)
        {
            this.UserId = userId;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }

    }
}

