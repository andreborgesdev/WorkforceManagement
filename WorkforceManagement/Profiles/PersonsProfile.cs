using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Entities;
using WorkforceManagement.Helpers;
using WorkforceManagement.Models;

namespace WorkforceManagement.Profiles
{
    public class PersonsProfile : Profile
    {
        public PersonsProfile()
        {
            CreateMap<Person, PersonDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

            CreateMap<PersonForCreationDto, Person>();
        }
    }
}
