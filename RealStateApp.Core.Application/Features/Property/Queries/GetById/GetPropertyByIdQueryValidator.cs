using FluentValidation;

namespace RealStateApp.Core.Application.Features.Property.Queries.GetById;

public class GetPropertyByIdQueryValidator : AbstractValidator<GetPropertyByIdQuery>
{
    public GetPropertyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id debe de ser mayor que cero.")
            .NotNull().WithMessage("El Id es requerido");
    }
    
}