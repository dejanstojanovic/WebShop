using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebShop.Users.Dtos.ApplicationUser
{
    /// <summary>
    /// User password update DTO
    /// </summary>
    public class PasswordUpdate
    {
        /// <summary>
        /// User's current password
        /// </summary>
        [Required(ErrorMessage = "Old password value is mandatory")]
        public String OldPassword { get; set; }

        /// <summary>
        /// New password to be set for the user
        /// </summary>
        [Required(ErrorMessage = "New password value is mandatory")]
        public String NewPassword { get; set; }
    }
}
