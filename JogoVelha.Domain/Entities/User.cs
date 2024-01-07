using System.ComponentModel.DataAnnotations.Schema;
using JogoVelha.Domain.Entities.Base;

namespace JogoVelha.Domain.Entities;

public class User : EntityBase
{   
    [Column("username")]
    public string Username { get; set; }

    [Column("email")]
    public string Email { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
}