using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Models;

namespace WorkforceManagement.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(Url.Link("GetRoot", new { }),
                "self",
                "GET"));

            links.Add(
                new LinkDto(Url.Link("GetPersons", new { }),
                "persons",
                "GET"));

            links.Add(
                new LinkDto(Url.Link("CreatePerson", new { }),
                "create_person",
                "POST"));

            return Ok(links);
        }
    }
}
