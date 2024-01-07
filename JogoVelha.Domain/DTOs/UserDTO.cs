namespace JogoVelha.Domain.DTOs;

public sealed class UserDTO 
{   
    public record UserRequest(
        int Id,
        string Username,
        string Email,
        string Password
    );

    public record UserResponse(
        int Id,
        string Username,
        string Email,
        string Password
    );

}


