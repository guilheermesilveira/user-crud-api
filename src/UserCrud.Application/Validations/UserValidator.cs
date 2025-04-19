using FluentValidation;
using UserCrud.Domain.Models;

namespace UserCrud.Application.Validations;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name)
            .NotNull()
            .WithMessage("The name cannot be null")
            .Length(3, 50)
            .WithMessage("The name must contain between {MinLength} and {MaxLength} characters");

        RuleFor(u => u.Email)
            .NotNull()
            .WithMessage("Email cannot be null")
            .EmailAddress()
            .WithMessage("The email provided is not in a valid email format");

        RuleFor(u => u.Password)
            .NotNull()
            .WithMessage("Password cannot be null")
            .MinimumLength(5)
            .WithMessage("Password must contain at least {MinLength} characters");
    }
}