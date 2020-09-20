using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WorkforceManagement.ActionConstraints;
using WorkforceManagement.Entities;
using WorkforceManagement.Helpers;
using WorkforceManagement.Models;
using WorkforceManagement.ResourceParameters;
using WorkforceManagement.Services;
using WorkforceManagement.Services.Interfaces;

namespace WorkforceManagement.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public PersonsController(IPersonRepository personRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _personRepository = personRepository ?? 
                throw new ArgumentNullException(nameof(personRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));

            _propertyCheckerService = propertyCheckerService ??
                throw new ArgumentNullException(nameof(propertyCheckerService));
        }


        // The FromQuery attribute is not necessary
        [HttpGet(Name = "GetPersons")]
        [HttpHead]
        public IActionResult GetPersons([FromQuery] PersonsResourceParameters personsResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<PersonDto, Person>(personsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<PersonDto>(personsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var peopleFromRepo = _personRepository.GetPersons(personsResourceParameters);

            //var previousPageLink = peopleFromRepo.HasPrevious ?
            //    CreatePersonsResourceUri(personsResourceParameters, ResourceUriType.PreviousPage) : null;

            //var nextPageLink = peopleFromRepo.HasNext ?
            //    CreatePersonsResourceUri(personsResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = peopleFromRepo.TotalCount,
                pageSize = peopleFromRepo.PageSize,
                currentPage = peopleFromRepo.CurrentPage,
                totalPages = peopleFromRepo.TotalPages
            };

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

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var links = CreateLinksForPersons(personsResourceParameters, peopleFromRepo.HasNext, peopleFromRepo.HasPrevious);

            var shapedPersons = _mapper.Map<IEnumerable<PersonDto>>(peopleFromRepo)
                .ShapeData(personsResourceParameters.Fields);

            var shapedPersonsWithLinks = shapedPersons.Select(person =>
            {
                var personAsDictionary = person as IDictionary<string, object>;
                var personLinks = CreateLinksForPerson((Guid)personAsDictionary["Id"], null);
                personAsDictionary.Add("links", personLinks);
                return personAsDictionary;
            });

            var linkedCollectionResource = new
            {
                value = shapedPersonsWithLinks,
                links
            };

            return Ok(linkedCollectionResource);
        }

        // These are all the types allowed for that action.
        // Be careful: Any type not on this list will return a 406
        // This can be applied at controller or application level as well
        [Produces("application/json",
            "application/vnd.marvin.hateoas+json",
            "application/vnd.marvin.person.full+json",
            "application/vnd.marvin.person.full.hateoas+json",
            "application/vnd.marvin.person.friendly+json",
            "application / vnd.marvin.person.friendly.hateoas+json")]
        [HttpGet("{personId}", Name = "GetPerson")]
        public ActionResult<PersonDto> GetPerson(Guid personId,
            string fields,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                out MediaTypeHeaderValue parseMediaType))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<PersonDto>(fields))
            {
                return BadRequest();
            }

            var personFromRepo = _personRepository.GetPerson(personId);

            if (personFromRepo == null)
            {
                return NotFound();
            }

            var includeLinks = parseMediaType.SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            IEnumerable<LinkDto> links = new List<LinkDto>();

            if (includeLinks)
            {
                links = CreateLinksForPerson(personId, fields);
            }

            var primaryMediaType = includeLinks ?
                parseMediaType.SubTypeWithoutSuffix
                .Substring(0, parseMediaType.SubTypeWithoutSuffix.Length - 8) 
                : parseMediaType.SubTypeWithoutSuffix;

            // Full Person
            if (primaryMediaType == "vnd.marvin.person.full")
            {
                var fullResourceToReturn = _mapper.Map<PersonFullDto>(personFromRepo)
                    .ShapeData(fields) as IDictionary<string, object>;

                if (includeLinks)
                {
                    fullResourceToReturn.Add("links", links);
                }

                return Ok(fullResourceToReturn);
            }

            // Friendly Person
            var friendlyResourceToReturn = _mapper.Map<PersonDto>(personFromRepo)
                .ShapeData(fields) as IDictionary<string, object>;

            if (includeLinks)
            {
                friendlyResourceToReturn.Add("links", links);
            }

            return Ok(friendlyResourceToReturn);
        }

        [HttpPost(Name = "CreatePersonWithDateOfDeath")]
        [RequestHeaderMatchesMediaType("Content-type",
            "application/vnd.marvin.personforcreationwithdateofdeath+json")]
        [Consumes("application/vnd.marvin.personforcreationwithdateofdeath+json")]
        public ActionResult<PersonDto> CreatePersonWithDateOfDeath(PersonForCreationWithDateOfDeathDto person)
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

            var links = CreateLinksForPerson(personToReturn.Id, null);

            var linkedResourceToReturn = personToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetPerson",
                new { personId = linkedResourceToReturn["Id"] }
                , linkedResourceToReturn);
        }

        [HttpPost(Name = "CreatePerson")]
        [RequestHeaderMatchesMediaType("Content-type",
            "application/json",
            "application/vnd.marvin.personforcreation+json")]
        [Consumes("application/json",
            "application/vnd.marvin.personforcreation+json")]
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

            var links = CreateLinksForPerson(personToReturn.Id, null);

            var linkedResourceToReturn = personToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetPerson", new { personId = linkedResourceToReturn["Id"] }, linkedResourceToReturn);
        }

        [HttpOptions]
        public IActionResult GetPersonOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,PATCH");
            return Ok();
        }

        [HttpDelete("{personId}", Name = "DeletePerson")]
        public ActionResult DeletePerson(Guid personId)
        {
            // We don't use the PersonExists here because we need the entity
            // to pass to the delete repo method anyway, so we can save one DB call
            var personFromRepo = _personRepository.GetPerson(personId);

            if (personFromRepo == null)
            {
                return NotFound();
            }

            _personRepository.DeletePerson(personFromRepo);

            _personRepository.Save();

            return NoContent();
        }

        private string CreatePersonsResourceUri(PersonsResourceParameters personsResourceParameters, 
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetPersons",
                        new
                        {
                            fields = personsResourceParameters.Fields,
                            orderBy = personsResourceParameters.OrderBy,
                            pageNumber = personsResourceParameters.PageNumber - 1,
                            pageSize = personsResourceParameters.PageSize,
                            gender = personsResourceParameters.Gender,
                            searchQuery = personsResourceParameters.SearchQuery
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetPersons",
                        new
                        {
                            fields = personsResourceParameters.Fields,
                            orderBy = personsResourceParameters.OrderBy,
                            pageNumber = personsResourceParameters.PageNumber + 1,
                            pageSize = personsResourceParameters.PageSize,
                            gender = personsResourceParameters.Gender,
                            searchQuery = personsResourceParameters.SearchQuery
                        });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetPersons",
                        new
                        {
                            fields = personsResourceParameters.Fields,
                            orderBy = personsResourceParameters.OrderBy,
                            pageNumber = personsResourceParameters.PageNumber,
                            pageSize = personsResourceParameters.PageSize,
                            gender = personsResourceParameters.Gender,
                            searchQuery = personsResourceParameters.SearchQuery
                        });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForPerson(Guid personId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link("GetPerson", new { personId }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(new LinkDto(Url.Link("GetPerson", new { personId, fields }),
                    "self",
                    "GET"));
            }

            links.Add(new LinkDto(Url.Link("DeletePerson", new { personId }),
                "delete_person",
                "DELETE"));

            links.Add(new LinkDto(Url.Link("CreateMaterialForPerson", new { personId }),
                "create_material_for_person",
                "POST"));

            links.Add(new LinkDto(Url.Link("GetMaterialsForPerson", new { personId }),
                "materials",
                "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForPersons(PersonsResourceParameters personsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(CreatePersonsResourceUri(personsResourceParameters, ResourceUriType.Current), 
                "self",
                "GET"));

            if (hasNext)
            {
                links.Add(new LinkDto(CreatePersonsResourceUri(personsResourceParameters, ResourceUriType.NextPage),
                    "nextPage",
                    "GET"));
            }

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreatePersonsResourceUri(personsResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage",
                    "GET"));
            }

            return links;
        }

    }
}
