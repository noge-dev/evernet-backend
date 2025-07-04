using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class LogoutRequestDtoValidator : AbstractValidator<LogoutRequestDto>
{
    public LogoutRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Le token de rafraîchissement est requis.");
    }
}