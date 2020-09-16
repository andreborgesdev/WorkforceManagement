using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        [HttpPut("{materialId}")]
        public IActionResult UpdateMaterialForPerson(Guid personId, Guid materialId, MaterialForUpdateDto material)
        {
            if (!_materialRepository.PersonExists(personId))
            {
                return NotFound();
            }

            var materialFromRepo = _materialRepository.GetMaterial(personId, materialId);

            if (materialFromRepo == null)
            {
                var materialToAdd = _mapper.Map<Material>(material);
                materialToAdd.Id = materialId;

                _materialRepository.AddMaterial(personId, materialToAdd);

                _materialRepository.Save();

                var materialToReturn = _mapper.Map<MaterialDto>(materialToAdd);

                return CreatedAtRoute("GetMaterialForPerson",
                    new { personId, materialId = materialToReturn.Id },
                    materialToReturn); 
            }

            // Map the entity to a MaterialForUpdateDto
            _mapper.Map(material, materialFromRepo);

            // Apply the updated field values to that entity
            _materialRepository.UpdateMaterial(materialFromRepo);

            // By executing the mapper the entity has changed to a modified 
            // state and executing the save will write the changes to our database
            _materialRepository.Save();

            // Map the MaterialForUpdateDto back to an entity

            // We can return an OK or NoContent. The best one depends on the use case
            return NoContent();
        }

        [HttpPatch("{materialId}")]
        public ActionResult PartiallyUpdateMaterialForPerson(Guid personId,
            Guid materialId,
            JsonPatchDocument<MaterialForUpdateDto> patchDocument)
        {
            if (!_materialRepository.PersonExists(personId))
            {
                return NotFound();
            }

            var materialFromRepo = _materialRepository.GetMaterial(personId, materialId);

            if (materialFromRepo == null)
            {
                var materialDto = new MaterialForUpdateDto();

                patchDocument.ApplyTo(materialDto, ModelState);

                if (!TryValidateModel(materialDto))
                {
                    return ValidationProblem(ModelState);
                }

                var materialToAdd = _mapper.Map<Material>(materialDto);
                materialToAdd.Id = materialId;

                _materialRepository.AddMaterial(personId, materialToAdd);
                _materialRepository.Save();

                var materialToReturn = _mapper.Map<MaterialDto>(materialToAdd);

                return CreatedAtRoute("GetMaterialForPerson",
                    new { personId, materialId = materialToReturn.Id },
                    materialToReturn);
            }

            var materialToPatch = _mapper.Map<MaterialForUpdateDto>(materialFromRepo);

            patchDocument.ApplyTo(materialToPatch, ModelState);

            if (!TryValidateModel(materialToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(materialToPatch, materialFromRepo);

            _materialRepository.UpdateMaterial(materialFromRepo);

            _materialRepository.Save();

            return NoContent();
        }

        [HttpDelete("{materialId}")]
        public ActionResult DeleteMaterialForPerson(Guid personId, Guid materialId)
        {
            if (!_materialRepository.PersonExists(personId))
            {
                return NotFound();
            }

            var materialFromRepo = _materialRepository.GetMaterial(personId, materialId);

            if (materialFromRepo == null)
            {
                return NotFound();
            }

            _materialRepository.DeleteMaterial(materialFromRepo);
            _materialRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

    }
}
