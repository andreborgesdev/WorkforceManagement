using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Entities;
using WorkforceManagement.Helpers;
using WorkforceManagement.Models;
using WorkforceManagement.Services;

namespace WorkforceManagement.Controllers
{
    [ApiController]
    [Route("api/personcollections")]
    public class PersonCollectionsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonCollectionsController(IPersonRepository personRepository,
            IMapper mapper)
        {
            _personRepository = personRepository ??
                throw new ArgumentNullException(nameof(personRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetPersonCollection")]
        public IActionResult GetPersonCollection(
            [FromRoute] 
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var personEntities = _personRepository.GetPersons(ids);

            if (ids.Count() != personEntities.Count())
            {
                return NotFound();
            }

            var personsToReturn = _mapper.Map<IEnumerable<PersonDto>>(personEntities);

            return Ok(personsToReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<PersonDto>> CreatePersonCollection(IEnumerable<PersonForCreationDto> personCollection)
        {
            var personEntities = _mapper.Map<IEnumerable<Person>>(personCollection);

            foreach (var person in personEntities)
            {
                _personRepository.AddPerson(person);
            }

            _personRepository.Save();

            var personCollectionToReturn = _mapper.Map<IEnumerable<PersonDto>>(personEntities);
            var idsAsString = string.Join(",", personCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetPersonCollection", 
                new { ids = idsAsString },
                personCollectionToReturn);
        }
    }
}
