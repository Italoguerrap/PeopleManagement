using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PeopleManagement.API.Requests;
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
        public async Task<IActionResult> SearchAsync([FromQuery] GetPeopleQueryRequest? query, [FromServices] IValidator<GetPeopleQueryRequest> validator, IMapper mapper, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                return BadRequest("Query cannot be null.");
            }

            ValidationResult validationResult = await validator.ValidateAsync(query, opt => opt.IncludeRuleSets("v1"), cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            FilterCriteriaDto filter = mapper.Map<FilterCriteriaDto>(query);

            List<PersonDto> people = await peopleService.SearchAsync(filter, cancellationToken);

            return Ok(people);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreatePersonRequest createPersonRequest, [FromServices] IValidator<CreatePersonRequest> validator, IMapper mapper, CancellationToken cancellationToken)
        {
            if (createPersonRequest is null)
            {
                return BadRequest("Request cannot be null.");
            }

            ValidationResult validationResult = await validator.ValidateAsync(createPersonRequest, opt => opt.IncludeRuleSets("v1"), cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            PersonDto personDto = mapper.Map<PersonDto>(createPersonRequest);

            await peopleService.AddAsync(personDto, cancellationToken);
            return Ok();
        }


        [HttpPut("{cpf}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string cpf, [FromBody] UpdatePersonRequest updatePersonRequest, [FromServices] IValidator<UpdatePersonRequest> validator, IMapper mapper, CancellationToken cancellationToken)
        {
            if (updatePersonRequest is null)
            {
                return BadRequest("Request cannot be null.");
            }

            ValidationResult validationResult = await validator.ValidateAsync(updatePersonRequest, opt => opt.IncludeRuleSets("v1"), cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            PersonDto personDto = mapper.Map<PersonDto>(updatePersonRequest);


            PersonDto updated = await peopleService.UpdateAsync(cpf, personDto, cancellationToken);

            return Ok(updated);
        }

        [HttpDelete("{cpf}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string cpf, CancellationToken cancellationToken)
        {
            PersonDto deleted = await peopleService.DeleteAsync(cpf, cancellationToken);
            return Ok(deleted);
        }
    }
}