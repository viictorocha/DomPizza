namespace DomPizza.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public ICollection<RolePermissao> RolePermissoes { get; set; } = new List<RolePermissao>();
        public ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
    }
}
