using MercadoLivre_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoLivre_API.Data.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            // Tabela
            builder.ToTable("Produto");

            // Chave primária
            builder.HasKey(x => x.Id);

            // Identity para PostgreSQL
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseSerialColumn();

            // Propriedades
            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("Nome")
                .HasColumnType("text")
                .HasMaxLength(255);

            builder.Property(x => x.Preco)
                .IsRequired()
                .HasColumnName("Preco")
                .HasColumnType("numeric(10,2)");

            builder.Property(x => x.QuantidadeVenda)
                .IsRequired()
                .HasColumnName("QuantidadeVenda")
                .HasColumnType("integer");

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasConstraintName("FK_Categoria_Produto")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
