namespace MercadoLivre_API.ViewModels.ProdutoViewModel
{
    public class VisualizarProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeVenda { get; set; }
        public VisualizarCategoriaProdutoViewModel Categoria{ get; set; }
    }
}
