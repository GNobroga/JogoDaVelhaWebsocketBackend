using FluentValidation;
using JogoVelha.Domain.DTOs;

namespace JogoVelha.Application.Validations;

public class UserLoginValidator : AbstractValidator<UserDTO.UserLogin>
{
    public UserLoginValidator()
    {
        RuleFor(u => u.Email) 
            .EmailAddress().WithMessage("O email não é válido");

        RuleFor(u => u.Password) 
            .NotEmpty().WithMessage("A password não pode ser vazia");
    }
}