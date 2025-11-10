using DomPizza.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomPizza.Data.Context
{
    public static class DomPizzaContextSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // === Roles Padrão ===
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Nome = "Administrador", Descricao = "Acesso total ao sistema" },
                new Role { Id = 2, Nome = "Operador", Descricao = "Gestão de usuários e produtos" },
                new Role { Id = 3, Nome = "Leitor", Descricao = "Acesso somente leitura" }
            );

            // === Permissões Padrão ===
            modelBuilder.Entity<Permissao>().HasData(
                new Permissao { Id = 1, Tipo = "Usuario", Acao = "Ler", Descricao = "Pode visualizar usuários" },
                new Permissao { Id = 2, Tipo = "Usuario", Acao = "Criar", Descricao = "Pode criar usuários" },
                new Permissao { Id = 3, Tipo = "Usuario", Acao = "Editar", Descricao = "Pode editar usuários" },
                new Permissao { Id = 4, Tipo = "Usuario", Acao = "Excluir", Descricao = "Pode excluir usuários" }
            );

            // === Relacionamentos Role x Permissão ===
            modelBuilder.Entity<RolePermissao>().HasData(
                // Admin – todas permissões
                new RolePermissao { RoleId = 1, PermissaoId = 1 },
                new RolePermissao { RoleId = 1, PermissaoId = 2 },
                new RolePermissao { RoleId = 1, PermissaoId = 3 },
                new RolePermissao { RoleId = 1, PermissaoId = 4 },

                // Operador – leitura, criação e edição
                new RolePermissao { RoleId = 2, PermissaoId = 1 },
                new RolePermissao { RoleId = 2, PermissaoId = 2 },
                new RolePermissao { RoleId = 2, PermissaoId = 3 },

                // Leitor – apenas leitura
                new RolePermissao { RoleId = 3, PermissaoId = 1 }
            );

            // === Usuário Administrador ===
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Administrador DomPizza",
                    Email = "DomPizza@adm",
                    SenhaHash = "AQAAAAIAAYagAAAAEFyhC+iz5ABqnQQcYhzlmStnBaOGghPJOJPyK5jTrFo1Blt5ceMGtZ7+JuBSZ7daYg=="
                }
            );

            // === Relacionar Usuário ao Role de Administrador ===
            modelBuilder.Entity<UsuarioRole>().HasData(
                new UsuarioRole { UsuarioId = 1, RoleId = 1 }
            );
        }
    }
}
