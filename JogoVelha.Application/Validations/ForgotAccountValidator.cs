using FluentValidation;
using JogoVelha.Domain.DTOs;

namespace JogoVelha.Application.Validations;

public class ForgotAccountValidator: AbstractValidator<ForgotAccountDTO>
{
    public ForgotAccountValidator()
    {
         RuleFor(u => u.Email) 
            .EmailAddress().WithMessage("O email não é válido");

        RuleFor(u => u.Password) 
            .NotEmpty().WithMessage("A password não pode ser vazia");

        RuleFor(u => u.ConfirmPassword)
            .Equal(u => u.Password).WithMessage("A confirm password deve ser igual a password");
    }
}