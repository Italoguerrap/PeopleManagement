using PeopleManagement.Domain.Entities;

namespace PeopleManagement.Application.Interfaces;

public interface ITokenService
{
    Token GenerateToken(long userId);
}