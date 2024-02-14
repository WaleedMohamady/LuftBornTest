using LuftBornTest.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LuftBornTest.Application.Users;
public class RegisterCommand : IRequest<string>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; set; }
    public string Password { get; set; }
    public class Handler(UserManager<User> userManager) : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<User> _userManager = userManager;
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = new User
                {
                    UserName = request.FirstName + request.LastName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                };

                var creationResult = await _userManager.CreateAsync(newUser, request.Password);

                if (!creationResult.Succeeded)
                {
                    return creationResult.Errors.FirstOrDefault().Description;
                }

                var userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, newUser.UserName),
                    new Claim(ClaimTypes.Email, newUser.Email),
                    new Claim(ClaimTypes.Role,"User"),
                };

                await _userManager.AddClaimsAsync(newUser, userClaims);

                return "Done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
