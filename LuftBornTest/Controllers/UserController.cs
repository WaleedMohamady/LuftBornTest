using LuftBornTest.Application.Users;
using LuftBornTest.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LuftBornTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator, IConfiguration configuration, UserManager<User> userManager) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<User> _userManager = userManager;

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<string>> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return BadRequest("User not found");
        }
        if (await _userManager.IsLockedOutAsync(user))
        {
            return BadRequest("Try again");
        }

        bool isAuthenticated = await _userManager.CheckPasswordAsync(user, password);
        if (!isAuthenticated)
        {
            await _userManager.AccessFailedAsync(user);
            return Unauthorized("Wrong Credentials");
        }

        var userClaims = await _userManager.GetClaimsAsync(user);

        //Generate Key
        var secretKey = _configuration.GetValue<string>("SecretKey");
        var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKey);
        var key = new SymmetricSecurityKey(secretKeyInBytes);

        //Determine how to generate hashing result
        var methodUsedInGeneratingToken = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var exp = DateTime.Now.AddMinutes(15);

        //Genete Token 
        var jwt = new JwtSecurityToken(
            claims: userClaims,
            notBefore: DateTime.Now,
            issuer: "backendApplication",
            audience: "Products",
            expires: exp,
            signingCredentials: methodUsedInGeneratingToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        string tokenString = tokenHandler.WriteToken(jwt);
        return tokenString;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<string>> Register(RegisterCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
