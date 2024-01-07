using System.Data;
using FluentValidation;
using JogoVelha.Domain.DTOs;

namespace JogoVelha.Application.Validations;

public class UserValidator : AbstractValidator<UserDTO.UserRequest>
{
    public UserValidator() 
    {
        RuleFor(u => u.Username)
            .NotEmpty().WithMessage("O username é requerido")
            .MinimumLength(5).WithMessage("O username precisa ter no mínimo 5 caracteres")
            .MaximumLength(100).WithMessage("O username pode ter no máximo 100 caracteres");
        
        RuleFor(u => u.Email)
            .EmailAddress().WithMessage("O email não é válido")
            .MaximumLength(100).WithMessage("O username pode ter no máximo 100 caracteres");
        
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("A password é requirida")
            .MinimumLength(5).WithMessage("A password precisa ter no mínimo 5 caracteres")
            .MaximumLength(200).WithMessage("A password pode ter no máximo 200 caracteres");

        RuleFor(u => u.ConfirmPassword)
            .Equal(u => u.Password).WithMessage("A confirm password deve ser igual a password")
            .When(u => !string.IsNullOrEmpty(u.ConfirmPassword));

    }
}   