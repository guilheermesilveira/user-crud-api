using FluentValidation;
using FluentValidation.Results;

namespace UserCrud.Application.DTOs.User;

public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public bool Validate(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<UpdateUserDto>();

        if (!string.IsNullOrEmpty(Name))
        {
            validator
                .RuleFor(x => x.Name)
                .Length(3, 50)
                .WithMessage("O nome deve conter entre {MinLength} e {MaxLength} caracteres.");
        }

        if (!string.IsNullOrEmpty(Email))
        {
            validator
                .RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("O email fornecido não é válido.")
                .MaximumLength(100)
                .WithMessage("O email deve conter no máximo {MaxLength} caracteres.");
        }

        if (!string.IsNullOrEmpty(Password))
        {
            validator
                .RuleFor(x => x.Password)
                .MinimumLength(5)
                .WithMessage("A senha deve conter no mínimo {MinLength} caracteres.");
        }

        validationResult = validator.Validate(this);

        return validationResult.IsValid;
    }
}