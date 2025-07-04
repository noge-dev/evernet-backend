using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Le mot de passe actuel est requis.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Le nouveau mot de passe est requis.")
            .MinimumLength(8).WithMessage("Le nouveau mot de passe doit contenir au moins 8 caractères.");
    }
}