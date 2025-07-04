using Evernet.WebApi.DTOs;
using FluentValidation;

namespace Evernet.WebApi.Validators;

public class LoginResponseDtoValidator : AbstractValidator<LoginResponseDto>
{
    public LoginResponseDtoValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Le token d'accès est requis.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Le token de rafraîchissement est requis.");
    }
}