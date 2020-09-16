using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.ValidationAttributes;

namespace WorkforceManagement.Models
{
    [PersonContactMustBeDifferentFromEmail(ErrorMessage = "The provided contact should be different from the email")]
    public abstract class PersonForManipulationDto //: IValidatableObject
    {
        [Required(ErrorMessage = "You should fill out a first name.")]
        [MaxLength(50, ErrorMessage = "The first name shouldn't have more than 100 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "You should fill out a last name.")]
        [MaxLength(50, ErrorMessage = "The last name shouldn't have more than 100 characters.")]
        public string LastName { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        [Required]
        [MaxLength(15)]
        public string Contact { get; set; }
        [Required]
        [MaxLength(9)]
        public string NIF { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(10)]
        public virtual string Gender { get; set; }
        public ICollection<MaterialForCreationDto> Materials { get; set; }
            = new List<MaterialForCreationDto>();

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Email == Contact)
        //    {
        //        yield return new ValidationResult(
        //            "The provided contact should be different from the email",
        //            new[] { "PersonForCreationDto" });
        //    }
        //}
    }
}
