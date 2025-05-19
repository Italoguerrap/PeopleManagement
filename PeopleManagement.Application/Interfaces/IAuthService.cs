using PeopleManagement.Application.DTOs;

namespace PeopleManagement.Application.Interfaces;

public interface IAuthService
{
    Task<TokenDto> AuthenticateAsync(string cpf, string password, CancellationToken cancellationToken);

    bool IsValidToken(string token);
}
