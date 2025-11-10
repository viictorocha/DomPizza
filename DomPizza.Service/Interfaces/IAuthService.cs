using DomPizza.Domain.Entities;
using DomPizza.Service.DTOs;

namespace DomPizza.Service.Interfaces;

public interface IAuthService
{
    Task<string?> AutenticarAsync(LoginDTO dto);
}
