using DomPizza.Domain.Entities;
using DomPizza.Domain.Interfaces;
using DomPizza.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DomPizza.Data.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DomPizzaContext _context;

    public UsuarioRepository(DomPizzaContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AdicionarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }
    public async Task<Usuario?> ObterCompletoPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissoes)
                        .ThenInclude(rp => rp.Permissao)
            .FirstOrDefaultAsync(u => u.Email == email);
    }


}
