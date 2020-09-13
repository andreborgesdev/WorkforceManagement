using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkforceManagement.Models
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string NIF { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
}
