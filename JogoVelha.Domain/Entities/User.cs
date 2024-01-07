using System.ComponentModel.DataAnnotations.Schema;
using JogoVelha.Domain.Entities.Base;

namespace JogoVelha.Domain.Entities;

[Table("users")]
public class User : EntityBase
{   
    [Column("username")]
    public string Username { get; set; } = default!;

    [Column("email")]
    public string Email { get; set; } = default!;
    
    [Column("password")]
    public string Password { get; set; } = default!;
}