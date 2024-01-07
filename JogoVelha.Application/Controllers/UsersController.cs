using System.Net;
using Asp.Versioning;
using JogoVelha.Domain.DTOs;
using JogoVelha.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JogoVelha.Application.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service) : ControllerBase
{   
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDTO.UserResponse>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<UserDTO.UserResponse>>> Get()
    {
        return Ok(await service.FindAll());
    }

    [HttpGet("{id:int}", Name = "GetById")]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<UserDTO.UserResponse>> Get(int id)
    {
        return await service.FindById(id);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.Created)]
    public async Task<ActionResult<UserDTO.UserResponse>> Post(UserDTO.UserRequest record)
    {
        var userResponse = await service.Create(record);
        return CreatedAtRoute("GetById", new { Id = record.Id }, userResponse);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<UserDTO.UserResponse>> Put(int id, UserDTO.UserRequest record)
    {
        var userResponse = await service.Update(id, record);
        return userResponse;
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(UserDTO.UserResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        return await service.Delete(id);
    }

}