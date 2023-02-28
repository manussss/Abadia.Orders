namespace Abadia.Orders.Application.Commands.Authentication;

public class LoginCommand : IRequest<ResponseContract>
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}