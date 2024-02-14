using FluentValidation;

namespace LuftBornTest.Application.Products.Commands.Update;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
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
