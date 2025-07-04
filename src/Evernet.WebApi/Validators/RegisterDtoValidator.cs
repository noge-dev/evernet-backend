using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Le prénom est requis.")
            .MaximumLength(100).WithMessage("Le prénom ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Le second prénom ne peut pas dépasser 100 caractères.")
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Le nom est requis.")
            .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email est requis.")
            .EmailAddress().WithMessage("L'email doit être une adresse valide.")
            .MaximumLength(256).WithMessage("L'email ne peut pas dépasser 256 caractères.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Le mot de passe est requis.")
            .MinimumLength(8).WithMessage("Le mot de passe doit contenir au moins 8 caractères.")
            .MaximumLength(100).WithMessage("Le mot de passe ne peut pas dépasser 100 caractères.");
    }
}