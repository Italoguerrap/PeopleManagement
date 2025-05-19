namespace PeopleManagement.API.Responses;

public class AuthResponse
{
    public required string AccessToken { get; set; }
    public required DateTime Expiration { get; set; }
    public required string Cpf { get; set; }
}
