namespace DomPizza.Domain.Entities
{
    public class RolePermissao
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public int PermissaoId { get; set; }
        public Permissao Permissao { get; set; } = null!;
    }
}
