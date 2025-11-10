using DomPizza.Domain.Entities;
using DomPizza.Domain.Interfaces;
using DomPizza.Service.DTOs;
using DomPizza.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        var usuario = await _repo.ObterCompletoPorEmailAsync(dto.Email);
        // ↑ Este método precisa incluir Roles e Permissões:
        // Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r.RolePermissoes).ThenInclude(rp => rp.Permissao)

        if (usuario == null)
            return null;

        var hasher = new PasswordHasher<Usuario>();
        var result = hasher.VerifyHashedPassword(usuario, usuario.SenhaHash, dto.Senha);
        if (result == PasswordVerificationResult.Failed)
            return null;

        // === CRIAÇÃO DO TOKEN JWT ===
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        // === ADICIONAR ROLES ===
        var roles = usuario.UsuarioRoles.Select(ur => ur.Role.Nome).ToList();
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        // === ADICIONAR CLAIMS DE PERMISSÕES (opcional) ===
        var permissoes = usuario.UsuarioRoles
            .SelectMany(ur => ur.Role.RolePermissoes)
            .Select(rp => rp.Permissao)
            .Distinct();

        foreach (var p in permissoes)
            claims.Add(new Claim("Permissao", $"{p.Tipo}.{p.Acao}"));

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
