namespace PeopleManagement.Domain.Entities;

public class PeopleManagementException : Exception
{
    public PeopleManagementException()
    {
    }

    public PeopleManagementException(string? message) : base(message)
    {
    }

    public PeopleManagementException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
