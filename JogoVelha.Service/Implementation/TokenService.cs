using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Email)
        ];

        var token = new JwtSecurityToken(
            issuer: configuration.Issuer,
            signingCredentials: signingCrendetials,
            claims: claims,
            expires: DateTime.Now.AddMinutes(configuration.Duration)
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}