using MercadoLivre_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLivre_API.Data
{
    public class MercadoLivreDataContext : DbContext
    {
        public MercadoLivreDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost,5432;Database=MercadoLivre;User ID=postgres;Password=1234;TrustServerCertificate=True");
            }
        }
    }
}
