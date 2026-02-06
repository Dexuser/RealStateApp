using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.SaleType.Queries.GetAll;
/// <summary>
/// Query para obtener todos los tipos de venta.
/// </summary>
public class GetAllSaleTypesQuery : IRequest<List<SaleTypeApiDto>> {}

/// <summary>
/// Handler para obtener todos los tipos de venta.
/// </summary>
public class GetAllSaleTypesQueryHandler
    : IRequestHandler<GetAllSaleTypesQuery, List<SaleTypeApiDto>>
{
    private readonly ISaleTypeRepository _repository;
    private readonly IMapper _mapper;

    public GetAllSaleTypesQueryHandler(ISaleTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<SaleTypeApiDto>> Handle(GetAllSaleTypesQuery request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetAllAsync();
        return _mapper.Map<List<SaleTypeApiDto>>(list);
    }
}