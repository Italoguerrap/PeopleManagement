namespace PeopleManagement.API.Requests;

public class AuthRequest
{
    public string Cpf { get; set; } = null!;
    public string Password { get; set; } = null!;
}