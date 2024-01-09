using System.Net;
using JogoVelha.Domain.DTOs;
using JogoVelha.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JogoVelha.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ITokenService tokenService) : ControllerBase
{   
    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult> Post(UserDTO.UserLogin login) 
    {
        var (email, password) = login;

        var user = await userService.FindByEmail(email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new ArgumentException("Email ou senha incorretos");
        }

        var token = tokenService.GenerateToken(user);

        return Ok(new { token });
    }
}