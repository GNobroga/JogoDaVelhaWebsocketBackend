using System.Text;
using JogoVelha.Domain.Entities;
using JogoVelha.Service.Configuration;
using JogoVelha.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace JogoVelha.Service.Implementation;

public class TokenService(TokenConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user)
    {

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Secret));
        var signingCrendetials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


        return "";

    }
}