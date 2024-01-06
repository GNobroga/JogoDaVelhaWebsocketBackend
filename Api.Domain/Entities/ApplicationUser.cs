using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain.Entities;

[Table("users")]
public class ApplicationUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required(ErrorMessage = "The {0} was not provided")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "The {0} was not provided")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "The {0} was not provided")]
    public string? Password { get; set; }

}