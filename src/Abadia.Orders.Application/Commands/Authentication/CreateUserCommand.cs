namespace Abadia.Orders.Application.Commands.Authentication;

public class CreateUserCommand : IRequest<ResponseContract>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }

    public CreateUserCommand(string userName, string email, string password, bool isAdmin)
    {
        UserName = userName;
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
    }
}