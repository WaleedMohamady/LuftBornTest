using FluentValidation;

namespace LuftBornTest.Application.Products.Commands.Create;
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("This Field is Required");

        RuleFor(p => p.Description)
            .NotEmpty()
                .WithMessage("This Field is Required");

        RuleFor(p => p.Price)
            .NotEmpty()
                .WithMessage("This Field is Required")
            .GreaterThan(0)
                .WithMessage("Available Amount must be greater than 0");
    }
}
