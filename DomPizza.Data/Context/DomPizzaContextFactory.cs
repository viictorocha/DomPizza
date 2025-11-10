using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DomPizza.Data.Context
{
    public class DomPizzaContextFactory : IDesignTimeDbContextFactory<DomPizzaContext>
    {
        public DomPizzaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomPizzaContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=DomPizza;Trusted_Connection=True;TrustServerCertificate=True");
            return new DomPizzaContext(optionsBuilder.Options);
        }
    }
}
