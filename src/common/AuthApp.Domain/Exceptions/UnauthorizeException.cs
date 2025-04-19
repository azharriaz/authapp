namespace AuthApp.Domain.Exceptions;

public class UnauthorizeException : Exception
{
    public UnauthorizeException() : base("User was not found!")
    {

    }
}
