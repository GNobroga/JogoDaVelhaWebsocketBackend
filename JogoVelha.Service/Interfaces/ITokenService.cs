using JogoVelha.Domain.Entities;

namespace JogoVelha.Service.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}