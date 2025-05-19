using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManagement.API.Requests;
using PeopleManagement.API.Responses;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using System.Text.RegularExpressions;

namespace PeopleManagement.API.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[AllowAnonymous]
public class AuthController() : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AuthAsync([FromBody] AuthRequest authRequest, [FromServices] IAuthService authService, [FromServices] IValidator<AuthRequest> validator, CancellationToken cancellationToken)
    {
        if (authRequest == null)
        {
            return BadRequest("Login request cannot be null.");
        }

        ValidationResult validationResult = validator.Validate(authRequest);

        if (validationResult.IsValid)
        {
            TokenDto token = await authService.AuthenticateAsync(Regex.Replace(authRequest.Cpf, @"\D", ""), authRequest.Password, cancellationToken);
            if (token != null)
            {
                return Ok(new AuthResponse
                {
                    AccessToken = token.AccessToken,
                    Expiration = token.Expiration,
                    Cpf = token.Person.CPF,
                });
            }
            return Unauthorized("Invalid credentials.");
        }

        return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    }
}