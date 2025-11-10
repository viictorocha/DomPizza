namespace DomPizza.Domain.Entities
{
    public class Permissao
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;  // Ex: "Usuario", "Produto", "Pedido"
        public string Acao { get; set; } = string.Empty;  // Ex: "Ler", "Criar", "Editar", "Excluir"
        public string Descricao { get; set; } = string.Empty;

        public ICollection<RolePermissao> RolePermissoes { get; set; } = new List<RolePermissao>();
    }
}
