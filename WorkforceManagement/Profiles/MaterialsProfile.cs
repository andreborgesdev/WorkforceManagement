using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Entities;
using WorkforceManagement.Models;

namespace WorkforceManagement.Profiles
{
    public class MaterialsProfile : Profile
    {
        public MaterialsProfile()
        {
            CreateMap<Material, MaterialDto>();
            CreateMap<MaterialForCreationDto, Material>();
        }
    }
}
