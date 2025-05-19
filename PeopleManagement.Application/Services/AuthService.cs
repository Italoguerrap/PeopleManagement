using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Domain.Entities;

namespace PeopleManagement.Application.Services;

public class AuthService(IPersonRepository personRepository, IMapper mapper, IPasswordHasher<Person> hasher, ITokenService tokenService) : IAuthService
{
    public async Task<TokenDto> AuthenticateAsync(string cpf, string password, CancellationToken cancellationToken)
    {
        // Validar email e senha
        Person? person = await personRepository.GetByCpfAsync(cpf, cancellationToken);

        person ??= new Person { PasswordHash = string.Empty };

        PasswordVerificationResult passwordVerificationResult;

        try
        {
            passwordVerificationResult = hasher.VerifyHashedPassword(person, person.PasswordHash, password);
        }
        catch
        {
            passwordVerificationResult = PasswordVerificationResult.Failed;
        }

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new PeopleManagementException("Credenciais inválidas");
        }

        var x = tokenService.GenerateToken(person.Id);

        return new TokenDto
        {
            AccessToken = x.AccessToken,
            Expiration = x.Expiration,
            Person = mapper.Map<PersonDto>(person)
        };
    }

    public bool IsValidToken(string token)
    {
        throw new NotImplementedException();
    }
}
