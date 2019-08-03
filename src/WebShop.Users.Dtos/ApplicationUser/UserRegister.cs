using WebShop.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebShop.Users.Dtos.ApplicationUser
{
    /// <summary>
    /// User register DTO
    /// </summary>
    public class UserRegister
    {
        /// <summary>
        /// Unique user identifier
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "Empty guid cannot be used")]
        public Guid Id { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        [Required(ErrorMessage = "First name value is mandatory")]
        public String FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [Required(ErrorMessage = "Last name value is mandatory")]
        public String LastName { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        [Required(ErrorMessage = "Email address value is mandatory")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public String Email { get; set; }

        /// <summary>
        /// User's password for authentication
        /// </summary>
        [Required(ErrorMessage = "Password value is mandatory")]
        public String Password { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// User's occupation
        /// </summary>
        [MaxLength(length: 500, ErrorMessage = "Maximum length is 500 characters")]
        public String Occupation { get; set; }

        /// <summary>
        /// User's education
        /// </summary>
        [MaxLength(length: 500, ErrorMessage = "Maximum length is 500 characters")]
        public String Education { get; set; }
    }
}
