namespace Abadia.Orders.Domain.AuthenticationModels;

public class TokenModel
{
    public string Token { get; set; }
    public DateTime ValidTo { get; set; }
}