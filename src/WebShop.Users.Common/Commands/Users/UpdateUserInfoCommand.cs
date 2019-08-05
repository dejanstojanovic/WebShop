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
    public class UpdateUserInfoCommand:ICommand
    {
        [NotEmptyGuid]
        public Guid Id { get; }
        [Required]
        public String FirstName { get;  }
        [Required]
        public String LastName { get; }

        public DateTime DateOfBirth { get; }
        public String Occupation { get; }
        public String Education { get; }

        [JsonConstructor]
        public UpdateUserInfoCommand(Guid id, String firstName, String lastName, DateTime dateOfBirth, String occupation, String education)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Occupation = occupation;
            this.Education = education;
        }

    }
}
