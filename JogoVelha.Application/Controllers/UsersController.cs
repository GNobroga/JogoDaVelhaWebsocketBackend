using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace JogoVelha.Application.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    public ActionResult Get() => Ok("OK!");
}