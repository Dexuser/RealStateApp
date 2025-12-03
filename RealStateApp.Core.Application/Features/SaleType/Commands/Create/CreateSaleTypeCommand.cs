using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.SaleType.Commands.Create;

public class CreateSaleTypeCommand : IRequest<SaleTypeApiDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateSaleTypeCommandHandler 
    : IRequestHandler<CreateSaleTypeCommand, SaleTypeApiDto>
{
    private readonly ISaleTypeRepository _repository;
    private readonly IMapper _mapper;

    public CreateSaleTypeCommandHandler(ISaleTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SaleTypeApiDto> Handle(CreateSaleTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.SaleType
        {
            Name = request.Name,
            Description = request.Description!,
        };

        await _repository.AddAsync(entity);
        return _mapper.Map<SaleTypeApiDto>(entity);
    }
}