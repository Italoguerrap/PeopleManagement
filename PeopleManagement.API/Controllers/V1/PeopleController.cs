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
        public async Task<IActionResult> SearchAsync([FromQuery] string? query, CancellationToken cancellationToken)
        {
            List<PersonDto> people = await peopleService.SearchAsync(query, cancellationToken);
            return Ok(people);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PersonDto personDto, CancellationToken cancellationToken)
        {
            await peopleService.AddAsync(personDto, cancellationToken);
            return Ok();
        }


        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] PersonDto personDto, CancellationToken cancellationToken)
        {
            PersonDto updated = await peopleService.UpdateAsync(id, personDto, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            PersonDto deleted = await peopleService.DeleteAsync(id, cancellationToken);
            return Ok(deleted);
        }
    }
}