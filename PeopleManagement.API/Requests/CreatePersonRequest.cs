namespace PeopleManagement.API.Requests;

public class CreatePersonRequest : PersonRequest
{
    public string Password { get; set; }
}