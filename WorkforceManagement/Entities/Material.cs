using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkforceManagement.Entities
{
    public class Material
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Reference { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        
        public Material()
        {
        }
    }
}
