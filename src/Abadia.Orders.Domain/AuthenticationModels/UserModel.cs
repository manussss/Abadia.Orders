namespace Abadia.Orders.Domain.AuthenticationModels;

public class UserModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
}
