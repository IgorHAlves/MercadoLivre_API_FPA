using MercadoLivre_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoLivre_API.Data.Mappings
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            // Tabela
            builder.ToTable("Categoria");

            // Chave primária
            builder.HasKey(x => x.Id);

            // Identity para PostgreSQL
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseSerialColumn();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("Nome")
                .HasColumnType("text")
                .HasMaxLength(255);

            builder.HasMany(x => x.Produtos)
                .WithOne()
                .HasForeignKey(x => x.Categoria.Id)
                .HasConstraintName("FK_Pedido_PedidoItem")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
