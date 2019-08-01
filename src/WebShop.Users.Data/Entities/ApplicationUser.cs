using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebShop.Users.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {

        [MaxLength(100)]
        public String FirstName { get; set; }
        [MaxLength(200)]
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(500)]
        public String Occupation { get; set; }
        [MaxLength(500)]
        public String Education { get; set; }
    }
}
