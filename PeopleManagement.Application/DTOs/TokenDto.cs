namespace PeopleManagement.Application.DTOs;

public class TokenDto
{
    public required string AccessToken { get; set; }
    public required PersonDto Person { get; set; }
    public required DateTime Expiration { get; set; }
}
