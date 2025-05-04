using MercadoLivre_API.ViewModels.ProdutoViewModel;

namespace MercadoLivre_API.ViewModels.CategoriaViewModel
{
    public class VisualizarCategoriaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<VisualizarProdutoCategoriaViewModel> Produtos { get; set; } = new();
    }
}
