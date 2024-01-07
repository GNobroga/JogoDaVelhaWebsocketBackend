namespace JogoVelha.Domain.DTOs;

public record UserDTO(
    int Id,
    string Username,
    string Email,
    string Password
);