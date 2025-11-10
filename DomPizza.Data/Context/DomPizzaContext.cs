using DomPizza.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomPizza.Data.Context;

public class DomPizzaContext : DbContext
{
    public DomPizzaContext(DbContextOptions<DomPizzaContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permissao> Permissoes => Set<Permissao>();
    public DbSet<UsuarioRole> UsuarioRoles => Set<UsuarioRole>();
    public DbSet<RolePermissao> RolePermissoes => Set<RolePermissao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DomPizzaContextSeed.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UsuarioRole>()
            .HasKey(ur => new { ur.UsuarioId, ur.RoleId });

        modelBuilder.Entity<UsuarioRole>()
            .HasOne(ur => ur.Usuario)
            .WithMany(u => u.UsuarioRoles)
            .HasForeignKey(ur => ur.UsuarioId);

        modelBuilder.Entity<UsuarioRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UsuarioRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<RolePermissao>()
            .HasKey(rp => new { rp.RoleId, rp.PermissaoId });

        modelBuilder.Entity<RolePermissao>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissoes)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermissao>()
            .HasOne(rp => rp.Permissao)
            .WithMany(p => p.RolePermissoes)
            .HasForeignKey(rp => rp.PermissaoId);

    }
}
