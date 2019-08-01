using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Common.Validation
{

    /// <summary>
    /// Validates Guid value for Guid.Empty
    /// </summary>
    public class NotEmptyGuidAttribute : ValidationAttribute
    {

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public NotEmptyGuidAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessageAccessor"></param>
        public NotEmptyGuidAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        public NotEmptyGuidAttribute(string errorMessage) : base(errorMessage)
        {
        }

        /// <summary>
        /// Checks if the value is Guid.Empty value
        /// </summary>
        /// <param name="value">Guid value to be validated</param>
        /// <param name="validationContext">Current validation context</param>
        /// <returns>True if value is not Guid.Empty, otherwise False</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (
                (value is Guid && (Guid)value != Guid.Empty) ||
                (Guid.TryParse(value as String, out var guid) && guid != Guid.Empty))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(!String.IsNullOrWhiteSpace(this.ErrorMessage) ? this.ErrorMessage : "Invalid guid value");

        }
    }
}
