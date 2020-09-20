using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkforceManagement.Models
{
    public class PersonForCreationWithDateOfDeathDto : PersonForCreationDto
    {
        public DateTimeOffset? DateOfDeath { get; set; }
    }
}
