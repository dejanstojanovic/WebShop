using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebShop.Users.Common.Dtos.ApplicationUser
{
    /// <summary>
    /// User's profile update DTO
    /// </summary>
    public class UserInfoUpdateDto
    {
        /// <summary>
        /// User's first name
        /// </summary>
        [Required(ErrorMessage = "First name value is mandatory")]
        [MaxLength(length:100, ErrorMessage="Maximum length is 100 characters")]
        public String FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [Required(ErrorMessage = "Last name value is mandatory")]
        [MaxLength(length: 200, ErrorMessage = "Maximum length is 200 characters")]
        public String LastName { get; set; }

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
