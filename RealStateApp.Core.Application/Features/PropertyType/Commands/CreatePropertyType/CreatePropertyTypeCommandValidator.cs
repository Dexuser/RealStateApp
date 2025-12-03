using FluentValidation;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.PropertyType.Commands.CreatePropertyType;

public class CreatePropertyTypeCommandValidator : AbstractValidator<CreatePropertyTypeCommand>
{
    public CreatePropertyTypeCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotNull().WithMessage("Description es required.")
            .MaximumLength(250).WithMessage("The Description must not exceed 250 characters.");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name es required.")
            .MaximumLength(70).WithMessage("The Name must not exceed 70 characters.");
    }
}