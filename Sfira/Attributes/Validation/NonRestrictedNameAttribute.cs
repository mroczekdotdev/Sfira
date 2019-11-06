using MroczekDotDev.Sfira.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MroczekDotDev.Sfira.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    sealed public class NonRestrictedNameAttribute : ValidationAttribute
    {
        private readonly HashSet<string> restrictedNames;

        public NonRestrictedNameAttribute()
        {
            restrictedNames = RestrictedNames.HashSet;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = value as string ?? "";

            if (restrictedNames.Contains(input) || input?.Length == 0)
            {
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName, input));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        public string GetErrorMessage(string displayName, string input)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, displayName, input);
        }
    }
}
