namespace PeopleManagement.Domain.Entities;

public class Token
{
    public required string AccessToken { get; set; }
    public required DateTime Expiration { get; set; }
    public required long UserId { get; set; }
}