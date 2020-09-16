using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkforceManagement.Models
{
    public abstract class MaterialForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a reference")]
        [MaxLength(100, ErrorMessage = "The reference shouldn't have more than 100 characters")]
        public string Reference { get; set; }
    }
}
