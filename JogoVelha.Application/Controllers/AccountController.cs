using System.Net;
using JogoVelha.Domain.DTOs;
using JogoVelha.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JogoVelha.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(IUserService userService, IAccountService accountService) : ControllerBase
{
    [HttpPost("create-account")]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.Created)]
    public async Task<ActionResult<UserDTO.UserResponse>> Post(UserDTO.UserRequest record)
    {
        var userResponse = await userService.Create(record);
        return CreatedAtRoute("GetById", new { Id = record.Id }, userResponse);
    }

    [HttpPatch("forgot-account")]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<UserDTO.UserResponse>> Patch(ForgotAccountDTO dto)
    {
        return await accountService.Update(dto);
    }

    [HttpPost("confirm-email")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult> Post(ConfirmEmailDTO dto)
    {
        var confirmed = (await userService.FindByEmail(dto.Email)) is not null;
        return Ok(new { confirmed });
    }

}