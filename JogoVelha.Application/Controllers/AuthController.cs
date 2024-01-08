using JogoVelha.Domain.DTOs;
using JogoVelha.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JogoVelha.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IUserService userService, ITokenService tokenService) : ControllerBase
{   
    [HttpPost]
    public async Task<ActionResult> Post(UserDTO.UserLogin login) 
    {
        var (email, password) = login;

        var user = await userService.FindByEmail(email);

        if (user is null || BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new ArgumentException("Email ou senha incorretos");
        }

        var token = tokenService.GenerateToken(user);

        return Ok(new { token });
    }
}