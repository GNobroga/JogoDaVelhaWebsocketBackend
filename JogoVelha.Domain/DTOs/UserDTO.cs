using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace JogoVelha.Domain.DTOs;

public sealed class UserDTO 
{   

    public class UserRequest : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O {0} é requirido.")]
        public string Username { get; set; } = default!;

        [EmailAddress(ErrorMessage = "O campo {0} não contém um endereço de e-mail válido.")]
        [Required(ErrorMessage = "O {0} é requirido.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "O {0} é requirido.")]
        public string Password { get; set; } = default!;

        public string? ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(ConfirmPassword) && !string.Equals(ConfirmPassword, Password)) 
            {
                yield return new ValidationResult(
                    errorMessage: "A senha de confirmação precisa ser igual a senha",
                    memberNames: [nameof(ConfirmPassword)]
                );
            }
        }
    }

    public record UserResponse(
        int Id,
        string Username,
        string Email,
        string Password
    );
}


