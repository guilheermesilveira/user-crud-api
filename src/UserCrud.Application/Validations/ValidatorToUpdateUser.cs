using FluentValidation;
using UserCrud.Domain.Models;

namespace UserCrud.Application.Validations;

public class ValidatorToUpdateUser : AbstractValidator<User>
{
    public ValidatorToUpdateUser()
    {
        RuleFor(u => u.Name)
            .Length(3, 50)
            .WithMessage("O nome deve conter entre {MinLength} e {MaxLength} caracteres.")
            .When(u => !string.IsNullOrEmpty(u.Name));

        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("O email fornecido não é válido.")
            .MaximumLength(100)
            .WithMessage("O email deve conter no máximo {MaxLength} caracteres.")
            .When(u => !string.IsNullOrEmpty(u.Email));

        RuleFor(u => u.Password)
            .MinimumLength(5)
            .WithMessage("A senha deve conter no mínimo {MinLength} caracteres.")
            .When(u => !string.IsNullOrEmpty(u.Password));
    }
}