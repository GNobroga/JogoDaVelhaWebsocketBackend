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
            .NotEmpty().WithMessage("A password é requirida")
            .MinimumLength(5).WithMessage("A password precisa ter no mínimo 5 caracteres")
            .MaximumLength(200).WithMessage("A password pode ter no máximo 200 caracteres");

        RuleFor(u => u.ConfirmPassword)
            .Equal(u => u.Password).WithMessage("A senha de confirmação deve ser igual a password");
    }
}