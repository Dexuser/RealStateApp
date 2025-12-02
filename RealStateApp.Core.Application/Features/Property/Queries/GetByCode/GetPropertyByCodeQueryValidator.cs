using FluentValidation;

namespace RealStateApp.Core.Application.Features.Property.Queries.GetByCode;

public class GetPropertyByCodeQueryValidator : AbstractValidator<GetPropertyByCodeQuery>
{
    public GetPropertyByCodeQueryValidator()
    {
        RuleFor(x => x.Code)
            .Length(6).WithMessage("The length of the code should be of 6 characters.")
            .NotNull().WithMessage("The code is required");
    }
    
}