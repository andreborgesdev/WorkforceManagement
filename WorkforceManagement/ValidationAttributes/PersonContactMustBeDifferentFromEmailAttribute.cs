using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Models;

namespace WorkforceManagement.ValidationAttributes
{
    public class PersonContactMustBeDifferentFromEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var person = (PersonForManipulationDto)validationContext.ObjectInstance;

            if (person.Contact == person.Email)
            {
                return new ValidationResult(
                    ErrorMessage,
                    new[] { nameof(PersonForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
