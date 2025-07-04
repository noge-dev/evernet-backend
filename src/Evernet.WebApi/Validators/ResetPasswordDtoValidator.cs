using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Le token de réinitialisation est requis.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Le nouveau mot de passe est requis.")
            .MinimumLength(8).WithMessage("Le nouveau mot de passe doit contenir au moins 8 caractères.")
            .MaximumLength(100).WithMessage("Le nouveau mot de passe ne peut pas dépasser 100 caractères.");
    }
}