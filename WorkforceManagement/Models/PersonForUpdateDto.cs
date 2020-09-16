﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkforceManagement.Models
{
    public class PersonForUpdateDto : PersonForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a description")]
        public override string Gender { get => base.Gender; set => base.Gender = value; }
    }
}