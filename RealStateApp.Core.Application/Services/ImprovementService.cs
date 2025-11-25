using AutoMapper;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class ImprovementService : GenericServices<Improvement,ImprovementDto>,  IImprovementService
{
    public ImprovementService(IImprovementRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}