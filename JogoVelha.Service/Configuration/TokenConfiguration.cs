namespace JogoVelha.Service.Configuration;

public class TokenConfiguration
{
    public string Issuer { get; set; } = null!;

    public string Secret { get; set; } = null!;
}