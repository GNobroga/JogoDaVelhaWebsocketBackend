namespace JogoVelha.Domain.DTOs;

public sealed class UserDTO 
{   
    public record UserRequest(
        int Id,
        string Username,
        string Email,
        string Password,
        string ConfirmPassword
    );

    public record UserResponse(
        int Id,
        string Username,
        string Email
    );

    public record UserLogin(
        string Email,
        string Password
    );
}


