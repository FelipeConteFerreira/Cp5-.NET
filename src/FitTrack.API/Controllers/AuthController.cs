using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FitTrack.API.Controllers;

public sealed record TokenRequest(string Username, string Password);

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IConfiguration config) : ControllerBase
{
    [HttpPost("token")]
    public IActionResult CreateToken(TokenRequest request)
    {
        var username = config["Auth:DemoUsername"];
        var password = config["Auth:DemoPassword"];
        if (request.Username != username || request.Password != password)
            return Unauthorized(new { error = "Credenciais inválidas." });

        var claims = new[] { new Claim(ClaimTypes.Name, request.Username), new Claim(ClaimTypes.Role, "User") };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"], claims,
            expires: DateTime.UtcNow.AddMinutes(60), signingCredentials: credentials);
        return Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token), tokenType = "Bearer", expiresIn = 3600 });
    }
}
