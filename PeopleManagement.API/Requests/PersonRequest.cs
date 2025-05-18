namespace PeopleManagement.API.Requests;

public class PersonRequest
{
    public virtual string? Name { get; set; }
    public virtual string? CPF { get; set; }
    public virtual DateOnly? DateOfBirth { get; set; }
    public virtual string? Email { get; set; }
    public virtual string? Gender { get; set; }
    public virtual string? Naturality { get; set; }
    public virtual string? Nationality { get; set; }
    public virtual string? Address { get; set; }
}
