namespace JogoVelha.Domain.DTOs;

public record ForgotAccountDTO(
    string Email,
    string Password,
    string ConfirmPassword
);