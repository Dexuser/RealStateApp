namespace RealStateApp.Infrastructure.Persistence.Services;

public interface ICodeService
{
    Task<string> GenerateIdentifier();
}