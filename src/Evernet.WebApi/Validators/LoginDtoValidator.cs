using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email est requis.")
            .EmailAddress().WithMessage("L'email doit être une adresse valide.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Le mot de passe est requis.");
    }
}