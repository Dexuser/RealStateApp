using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.SaleType.Queries.GetById;
public class GetSaleTypeByIdQuery : IRequest<SaleTypeApiDto?>
{
    public int Id { get; set; }
}

public class GetSaleTypeByIdQueryHandler 
    : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeApiDto?>
{
    private readonly ISaleTypeRepository _repository;
    private readonly IMapper _mapper;

    public GetSaleTypeByIdQueryHandler(ISaleTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SaleTypeApiDto?> Handle(GetSaleTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        return entity == null ? null : _mapper.Map<SaleTypeApiDto>(entity);
    }
}