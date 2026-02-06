using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.SaleType.Commands.Update;

public class UpdateSaleTypeCommand : IRequest<SaleTypeApiDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateSaleTypeCommandHandler(ISaleTypeRepository repository, IMapper mapper)
    : IRequestHandler<UpdateSaleTypeCommand, SaleTypeApiDto>
{
    public async Task<SaleTypeApiDto> Handle(UpdateSaleTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);

        entity!.Name = request.Name;
        entity.Description = request.Description!;

        await repository.UpdateAsync(entity.Id, entity);

        return mapper.Map<SaleTypeApiDto>(entity);
    }
}