using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManagement.API.Requests;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;

namespace PeopleManagement.API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class PeopleController(IPeopleService peopleService) : ControllerBase
    {
        protected virtual IPeopleService PeopleService { get; set; } = peopleService;
        [HttpGet]
        public virtual async Task<IActionResult> SearchAsync([FromQuery] GetPeopleQueryRequest? query, [FromServices] IValidator<GetPeopleQueryRequest> validator, IMapper mapper, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                return BadRequest("Query cannot be null.");
            }

            ValidationResult validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }));
            }

            FilterCriteriaDto filter = mapper.Map<FilterCriteriaDto>(query);

            List<PersonDto> people = await PeopleService.SearchAsync(filter, cancellationToken);

            return Ok(people);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<IActionResult> AddAsync([FromBody] CreatePersonRequest createPersonRequest, [FromServices] IValidator<CreatePersonRequest> validator, IMapper mapper, CancellationToken cancellationToken)
        {
            if (createPersonRequest is null)
            {
                return BadRequest("Request cannot be null.");
            }

            ValidationResult validationResult = await validator.ValidateAsync(createPersonRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }));
            }

            PersonDto personDto = mapper.Map<PersonDto>(createPersonRequest);

            await PeopleService.AddAsync(personDto, cancellationToken);
            return Ok();
        }


        [HttpPut("{cpf}")]
        public virtual async Task<IActionResult> UpdateAsync([FromRoute] string cpf, [FromBody] UpdatePersonRequest updatePersonRequest, [FromServices] IValidator<UpdatePersonRequest> validator, IMapper mapper, CancellationToken cancellationToken)
        {
            if (updatePersonRequest is null)
            {
                return BadRequest("Request cannot be null.");
            }

            ValidationResult validationResult = await validator.ValidateAsync(updatePersonRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }));
            }

            PersonDto personDto = mapper.Map<PersonDto>(updatePersonRequest);


            PersonDto updated = await PeopleService.UpdateAsync(cpf, personDto, cancellationToken);

            return Ok(updated);
        }

        [HttpDelete("{cpf}")]
        public virtual async Task<IActionResult> DeleteAsync([FromRoute] string cpf, CancellationToken cancellationToken)
        {
            PersonDto deleted = await PeopleService.DeleteAsync(cpf, cancellationToken);
            return Ok(deleted);
        }
    }
}