namespace DomPizza.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();

}

