using Microsoft.AspNetCore.Mvc;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;

namespace PeopleManagement.API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PeopleController(IPeopleService peopleService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string? query, CancellationToken cancellationToken)
        {
            List<PersonDto> people = await peopleService.SearchAsync(query, cancellationToken);

            return Ok(people);
        }
    }
}