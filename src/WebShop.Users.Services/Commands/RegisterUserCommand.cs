using WebShop.Users.Dtos.ApplicationUser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Services.Commands
{
    public class RegisterUserCommand : ICommand
    {
        public Guid Id { get; set; }
        public String FirstName { get; }
        public String LastName { get; }
        public String Email { get; }
        public String Password { get; }
        public DateTime DateOfBirth { get;  }
        public String Occupation { get; }
        public String Education { get;  }
        public byte[] Image { get; }
        [JsonConstructor]
        public RegisterUserCommand(Guid id, String firstName, String lastName, DateTime dateOfBirth, String occupation, String education,  String email, String password, byte[] image = null)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
            this.DateOfBirth = dateOfBirth;
            this.Occupation = occupation;
            this.Education = education;
            this.Image = image;
        }

        public RegisterUserCommand(Register user, byte[] image=null)
        {
            user.Id = user.Id != Guid.Empty ? user.Id : Guid.NewGuid();

            this.Id = user.Id;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Password = user.Password;
            this.DateOfBirth = user.DateOfBirth;
            this.Occupation = user.Occupation;
            this.Education = user.Education;
            this.Image = image;
        }

    }
}
