using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Api.Security;

public class JwtProvider
{
    public string? Secret { get; set; }

    public string? Issuer { get; set; }

     public string GenerateToken(string userId)
    {
        var expiryIn = DateTime.Now.AddMinutes(10);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            expires: expiryIn,
            signingCredentials: credentials,
            claims: [new Claim(JwtRegisteredClaimNames.Sub, userId)]
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }

}
