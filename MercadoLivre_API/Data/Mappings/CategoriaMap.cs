using MercadoLivre_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoLivre_API.Data.Mappings
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            //Tabela
            builder.ToTable("Categoria");
            //Chave primária
            builder.HasKey(x => x.Id);
            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("Nome")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);

            builder.HasMany(x => x.Produtos)
                .WithOne()
                .HasForeignKey(x => x.Categoria.Id)
                .HasConstraintName("FK_Pedido_PedidoItem")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
