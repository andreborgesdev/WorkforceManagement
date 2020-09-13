using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Entities;
using WorkforceManagement.Models;
using WorkforceManagement.Services;

namespace WorkforceManagement.Controllers
{
    [ApiController]
    [Route("api/persons/{personId}/materials")]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMapper _mapper;

        public MaterialsController(IMaterialRepository materialRepository,
            IMapper mapper)
        {
            _materialRepository = materialRepository ??
                throw new ArgumentNullException(nameof(materialRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<MaterialDto>> GetMaterialsForPerson(Guid personId)
        {
            if (!_materialRepository.PersonExists(personId))
            {
                return NotFound();
            }

            var materialsForPersonFromRepo = _materialRepository.GetMaterials(personId);

            return Ok(_mapper.Map<IEnumerable<MaterialDto>>(materialsForPersonFromRepo));
        }

        [HttpGet("{materialId}", Name ="GetMaterialForPerson")]
        public ActionResult<MaterialDto> GetMaterialForPerson(Guid personId, Guid materialId)
        {
            if (!_materialRepository.PersonExists(personId))
            {
                return NotFound();
            }

            var materialForPersonFromRepo = _materialRepository.GetMaterial(personId, materialId);

            if (materialForPersonFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MaterialDto>(materialForPersonFromRepo));
        }

        [HttpPost]
        public ActionResult<MaterialDto> CreateMaterialForPerson(Guid personId, MaterialForCreationDto material)
        {
            if (!_materialRepository.PersonExists(personId))
            {
                return NotFound();
            }

            var materialEntity = _mapper.Map<Material>(material);
            _materialRepository.AddMaterial(personId, materialEntity);
            _materialRepository.Save();

            var materialToReturn = _mapper.Map<MaterialDto>(materialEntity);

            return CreatedAtRoute("GetMaterialForPerson",
                new { personId = personId, materialId = materialToReturn.Id },
                materialToReturn);
        }
    }
}
