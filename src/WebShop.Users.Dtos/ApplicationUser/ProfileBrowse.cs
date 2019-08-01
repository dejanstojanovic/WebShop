using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Users.Dtos.ApplicationUser
{
    /// <summary>
    /// User's profile view DTO
    /// </summary>
    public class ProfileBrowse
    {
        /// <summary>
        /// User's first name
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Users's last name
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// User's occupation
        /// </summary>
        public String Occupation { get; set; }

        /// <summary>
        /// User's education
        /// </summary>
        public String Education { get; set; }

        /// <summary>
        /// Page index to be returned (starting from 0)
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Number of item s to be returned in a result
        /// </summary>
        public int PageSize { get; set; }
    }
}
