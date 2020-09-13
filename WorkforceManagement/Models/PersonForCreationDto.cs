using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkforceManagement.Models
{
    public class PersonForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string NIF { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public ICollection<MaterialForCreationDto> Materials { get; set; }
            = new List<MaterialForCreationDto>();
    }
}
