using Abadia.Orders.Domain.AuthenticationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Abadia.Orders.Application.Commands.Authentication;

public class AuthenticationCommandHandler : IRequestHandler<CreateUserCommand, ResponseContract>, IRequestHandler<LoginCommand, ResponseContract>
{
    private readonly ILogger<AuthenticationCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticationCommandHandler(
        ILogger<AuthenticationCommandHandler> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<ResponseContract> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requested user creation, email: {email}", request.Email);

        var response = new ResponseContract();
        var identityUser = await _userManager.FindByEmailAsync(request.Email) ?? await _userManager.FindByNameAsync(request.UserName);

        if (identityUser != null)
        {
            _logger.LogInformation("UserName or Email already exists, email: {email}, userName: {userName}", request.Email, request.UserName);

            response.SetResponse(false, HttpStatusCode.BadRequest, "UserName or Email already exists");
            
            return response;
        }

        var user = new IdentityUser
        {
            UserName = request.UserName,
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var identityResult = await _userManager.CreateAsync(user, request.Password);

        if (!identityResult.Succeeded)
        {
            _logger.LogInformation("An error occurred while creating user, email: {email}, error: {error}", request.Email, identityResult.Errors.FirstOrDefault()?.Description);

            response.SetResponse(false, HttpStatusCode.BadRequest, identityResult.Errors.FirstOrDefault()?.Description);

            return response;
        }

        var role = request.IsAdmin ? UserRoles.Admin : UserRoles.User;
        await AddToRoleAsync(user, role);

        _logger.LogInformation("User created with success, email: {email}", request.Email);

        response.SetResponse(true, HttpStatusCode.Created);

        return response;
    }

    public async Task<ResponseContract> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requested login, userName: {userName}", request.UserName);

        var response = new ResponseContract();
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is not null && await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                authClaims.Add(new(ClaimTypes.Role, role));

            var token = GetToken(authClaims);

            _logger.LogInformation("Login succeeded, userName: {userName}", request.UserName);

            response.SetResponse(true, HttpStatusCode.OK);
            response.SetData(token);

            return response;
        }

        _logger.LogInformation("Login denied, UserName or Password is invalid, userName: {userName}", request.UserName);

        response.SetResponse(false, HttpStatusCode.Unauthorized, "UserName or Password is invalid.");

        return response;
    }

    private TokenModel GetToken(List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return new TokenModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ValidTo = token.ValidTo
        };
    }

    private async Task AddToRoleAsync(IdentityUser user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new(role));

        await _userManager.AddToRoleAsync(user, role);
    }
}