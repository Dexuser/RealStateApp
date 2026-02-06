using MediatR;
using RealStateApp.Core.Domain.Interfaces;


namespace RealStateApp.Core.Application.Features.SaleType.Commands.Delete;

public class DeleteSaleTypeCommand : IRequest<string>
{
    public int Id { get; set; }
}

public class DeleteSaleTypeCommandHandler(ISaleTypeRepository repository)
    : IRequestHandler<DeleteSaleTypeCommand, string>
{
    public async Task<string> Handle(DeleteSaleTypeCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return "Tipo de venta eliminado correctamente.";
    }
}