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
}
