using MercadoLivre_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoLivre_API.Data.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            //Tabela
            builder.ToTable("Produto");
            //Chave primária
            builder.HasKey(x => x.Id);
            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Propriedades
            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("Nome")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.Preco)
                .IsRequired()
                .HasColumnName("Preco")
                .HasColumnType("DECIMAL(10,2)");//Validar

            builder.Property(x => x.QuantidadeVenda)
                .IsRequired()
                .HasColumnName("QuantidadeVenda")
                .HasColumnType("INTEGER");

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasConstraintName("FK_Categoria_Produto")
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
