using WebShop.Users.Dtos.ApplicationUser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Services.Commands
{
    public class UpdateProfileCommand:ICommand
    {
        public Guid Id { get; }
        public String FirstName { get;  }
        public String LastName { get; }
        public DateTime DateOfBirth { get; }
        public String Occupation { get; }
        public String Education { get; }

        [JsonConstructor]
        public UpdateProfileCommand(Guid id, String firstName, String lastName, DateTime dateOfBirth, String occupation, String education)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Occupation = occupation;
            this.Education = education;
        }

        public UpdateProfileCommand(Guid id,ProfileUpdate profileUpdate)
        {
            this.Id = id;
            this.FirstName = profileUpdate.FirstName;
            this.LastName = profileUpdate.LastName;
            this.DateOfBirth = profileUpdate.DateOfBirth;
            this.Occupation = profileUpdate.Occupation;
            this.Education = profileUpdate.Education;

        }
    }
}
