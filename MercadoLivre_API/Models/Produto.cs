using System.ComponentModel;

namespace MercadoLivre_API.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; } = 0m;
        public Categoria Categoria{ get; set; } = new Categoria();
        public int QuantidadeVenda { get; set; } = 0;
    }
}
