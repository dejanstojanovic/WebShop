using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using System.IO;

namespace WebShop.Common.Validation
{
    public class AllowedFileTypesAttribute: ValidationAttribute
    {
        private readonly IEnumerable<String> _allowedFileTypes;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AllowedFileTypesAttribute(String[] fileTypes)
        {
            this._allowedFileTypes = fileTypes;
        }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AllowedFileTypesAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessageAccessor"></param>
        public AllowedFileTypesAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        public AllowedFileTypesAttribute(string errorMessage) : base(errorMessage)
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
            IFormFile file = value as IFormFile;
            if ((_allowedFileTypes==null && !_allowedFileTypes.Any()) || (file != null && _allowedFileTypes.Select(e=>e.ToLower()).Contains(Path.GetExtension(file.FileName))))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(!String.IsNullOrWhiteSpace(this.ErrorMessage) ? this.ErrorMessage : "File type not allowed");

        }
    }
}
