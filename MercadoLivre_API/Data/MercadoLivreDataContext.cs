using MercadoLivre_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLivre_API.Data
{
    public class MercadoLivreDataContext : DbContext
    {
        public DbSet<Produto> Produtos{ get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MercadoLivre;User ID=SA;Password=1q2w3e4r@#$;TrustServerCertificate=True");
        }
    }
}
