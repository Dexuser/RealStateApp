namespace RealStateApp.Core.Application.Exceptions;

public class AppException : Exception
{
    public AppException() {}
    
    public AppException(string message) : base(message)
    {
    }
}
