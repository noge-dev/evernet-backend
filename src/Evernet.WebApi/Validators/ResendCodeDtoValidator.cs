using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class ResendCodeDtoValidator : AbstractValidator<ResendCodeDto>
{
    public ResendCodeDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("L'identifiant de l'utilisateur est requis.");
    }
}