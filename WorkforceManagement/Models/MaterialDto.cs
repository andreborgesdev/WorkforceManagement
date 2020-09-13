using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkforceManagement.Models
{
    public class MaterialDto
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public Guid PersonId { get; set; }
    }
}
