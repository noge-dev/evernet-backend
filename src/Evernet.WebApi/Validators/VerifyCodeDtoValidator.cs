using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class VerifyCodeDtoValidator : AbstractValidator<VerifyCodeDto>
{
    public VerifyCodeDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("L'identifiant de l'utilisateur est requis.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Le code est requis.")
            .Length(6).WithMessage("Le code doit contenir exactement 6 caractères.");
    }
}