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
    public class RegisterUserCommand : ICommand
    {
        public Guid Id { get; set; }
        [Required]
        public String FirstName { get; }
        [Required]
        public String LastName { get; }
        [Required]
        [EmailAddress]
        public String Email { get; }
        [Required]
        public String Password { get; }

        public DateTime DateOfBirth { get;  }
        public String Occupation { get; }
        public String Education { get;  }
        public byte[] Image { get; }
        [JsonConstructor]
        public RegisterUserCommand(Guid id, String firstName, String lastName, DateTime dateOfBirth, String occupation, String education,  String email, String password, byte[] image = null)
        {
            this.Id = id==Guid.Empty ? Guid.NewGuid(): id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
            this.DateOfBirth = dateOfBirth;
            this.Occupation = occupation;
            this.Education = education;
            this.Image = image;
        }


    }
}
