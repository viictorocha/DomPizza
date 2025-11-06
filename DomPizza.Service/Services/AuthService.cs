using DomPizza.Domain.Entities;
using DomPizza.Domain.Interfaces;
using DomPizza.Service.DTOs;
using DomPizza.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DomPizza.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repo;
    private readonly IConfiguration _config;

    public AuthService(IUsuarioRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<string?> AutenticarAsync(LoginDTO dto)
    {
        var usuario = await _repo.ObterPorEmailAsync(dto.Email);
        if (usuario == null) return null;

        var hasher = new PasswordHasher<Usuario>();
        //var senha = hasher.HashPassword(usuario, "string");

        var result = hasher.VerifyHashedPassword(null, usuario.SenhaHash, dto.Senha);
        if (result == PasswordVerificationResult.Failed) return null;

        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Email, usuario.Email)
    };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
