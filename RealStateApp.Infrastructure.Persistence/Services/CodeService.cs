using Microsoft.EntityFrameworkCore;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Services;

public class CodeService : ICodeService
{
    private readonly RealStateAppContext  _context;

    public CodeService(RealStateAppContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateIdentifier()
    {
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT NEXT VALUE FOR dbo.SeqPropertyCode";
        var result = await command.ExecuteScalarAsync();
        return ((long)result).ToString("D6");
    }
}
