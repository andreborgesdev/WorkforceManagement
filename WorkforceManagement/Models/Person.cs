using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkforceManagement.Models
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        public string NIF { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Sex { get; set; }

        //public Guid Material { get; set; }

        //public List<Material> Materials { get; set; } = new List<Material>();

        public Person()
        {
        }
    }
}
