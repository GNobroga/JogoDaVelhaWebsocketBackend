using FluentValidation;
using JogoVelha.Domain.DTOs;

namespace JogoVelha.Application.Validations;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDTO>
{
    public ConfirmEmailValidator()
    {
        RuleFor(ce => ce.Email)
            .EmailAddress().WithMessage("O email não é válido");
    }
}