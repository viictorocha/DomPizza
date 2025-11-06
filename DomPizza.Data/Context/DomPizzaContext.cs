using DomPizza.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomPizza.Data.Context;

public class DomPizzaContext : DbContext
{
    public DomPizzaContext(DbContextOptions<DomPizzaContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
}
