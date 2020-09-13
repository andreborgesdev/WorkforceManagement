using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Entities;
using WorkforceManagement.Helpers;
using WorkforceManagement.Models;
using WorkforceManagement.ResourceParameters;
using WorkforceManagement.Services;

namespace WorkforceManagement.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonsController(IPersonRepository personRepository,
            IMapper mapper)
        {
            _personRepository = personRepository ?? 
                throw new ArgumentNullException(nameof(personRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        // The FromQuery attribute is not necessary
        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<PersonDto>> GetPersons([FromQuery] PersonsResourceParameters personsResourceParameters)
        {
            var peopleFromRepo = _personRepository.GetPersons(personsResourceParameters);

            // O mapper serve para não ter que andar a fazer isto para todos
            //var people = new List<PersonDto>();

            //foreach (var person in peopleFromRepo)
            //{
            //    people.Add(new PersonDto()
            //    {
            //        Id = person.Id,
            //        Name = $"{person.FirstName} {person.LastName}",
            //        Address = person.Address,
            //        Age = person.DateOfBirth.GetCurrentAge(),
            //        Contact = person.Contact,
            //        Email = person.Email,
            //        NIF = person.NIF,
            //        Sex = person.Sex
            //    });
            //}

            return Ok(_mapper.Map<IEnumerable<PersonDto>>(peopleFromRepo));
        }

        [HttpGet("{personId}", Name = "GetPerson")]
        public ActionResult<PersonDto> GetAuthor(Guid personId)
        {
            var personFromRepo = _personRepository.GetPerson(personId);

            if (personFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PersonDto>(personFromRepo));
        }

        [HttpPost]
        public ActionResult<PersonDto> CreatePerson(PersonForCreationDto person)
        {
            // Needed for older versions of .NET, now the framework does that serializtion valiation for us
            //if (person == null)
            //{
            //    return BadRequest();
            //}

            var personEntity = _mapper.Map<Person>(person);
            _personRepository.AddPerson(personEntity);
            _personRepository.Save();

            var personToReturn = _mapper.Map<PersonDto>(personEntity);

            return CreatedAtRoute("GetPerson", new { personId = personToReturn.Id }, personToReturn);
        }
    }
}
