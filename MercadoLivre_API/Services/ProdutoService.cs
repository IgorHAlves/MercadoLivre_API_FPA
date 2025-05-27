using MercadoLivre_API.Data;
using MercadoLivre_API.Exceptions;
using MercadoLivre_API.Models;
using MercadoLivre_API.ViewModels.ProdutoViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MercadoLivre_API.Services
{
    public class ProdutoService
    {
        private readonly MercadoLivreDataContext _dbContext;

        public ProdutoService([FromServices] MercadoLivreDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<VisualizarProdutoViewModel> VisualizarProdutos(int offset, int limit)
        {

            List<Produto>? produtos = _dbContext.Produtos.Include(x => x.Categoria).Skip(offset).Take(limit).OrderBy(produto => produto.Id).ToList();

            List<VisualizarProdutoViewModel> vms = produtos.Select(produto => new VisualizarProdutoViewModel()
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeVenda = produto.QuantidadeVenda,
                Categoria = new VisualizarCategoriaProdutoViewModel()
                {
                    Id = produto.Categoria.Id,
                    Nome = produto.Categoria.Nome
                }
            }).ToList();

            return vms;
        }

        public VisualizarProdutoViewModel VisualizarProduto(int id)
        {

            Produto? produto = _dbContext.Produtos.Include(x => x.Categoria).SingleOrDefault(produto => produto.Id == id);

            if (produto == null)
            {
                throw new NotFoundException("Produto não localizado");
            }

            VisualizarProdutoViewModel vm = new VisualizarProdutoViewModel()
            {
                Id = id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeVenda = produto.QuantidadeVenda,
                Categoria = new VisualizarCategoriaProdutoViewModel()
                {
                    Id = produto.Categoria.Id,
                    Nome = produto.Categoria.Nome
                }
            };

            return vm;
        }


        public int CadastrarProduto(PostPutProdutoViewModel vm)
        {
            Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == vm.IdCategoria);

            if (categoria == null)
                throw new NotFoundException("Categoria não encontrada");

            Produto produto = new Produto
            {
                Nome = vm.Nome,
                Preco = vm.Preco,
                QuantidadeVenda = vm.QuantidadeVenda,
                Categoria = categoria
            };

            _dbContext.Produtos.Add(produto);
            _dbContext.SaveChanges();

            return produto.Id;
        }

        public VisualizarProdutoViewModel EditarProduto(int idProduto, PostPutProdutoViewModel vm)
        {
            Produto? produto = _dbContext.Produtos.SingleOrDefault(produto => produto.Id == idProduto);

            if (produto == null)
                throw new NotFoundException("Produto não encontrado");

            Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == vm.IdCategoria);

            if (categoria == null)
                throw new NotFoundException("Categoria não encontrada");

            produto.Nome = vm.Nome;
            produto.Preco = vm.Preco;
            produto.QuantidadeVenda = vm.QuantidadeVenda;
            produto.Categoria = categoria;

            _dbContext.SaveChanges();

            return new VisualizarProdutoViewModel
            {
                Id = idProduto,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeVenda = produto.QuantidadeVenda,
                Categoria = new VisualizarCategoriaProdutoViewModel
                {
                    Id = categoria.Id,
                    Nome = categoria.Nome
                }
            };
        }

        public void DeletarProduto(int idProduto)
        {

            Produto? produto = _dbContext.Produtos.SingleOrDefault(produto => produto.Id == idProduto);

            if (produto == null)
                throw new NotFoundException("Produto não encontrado");

            _dbContext.Produtos.Remove(produto);

            _dbContext.SaveChanges();
        }

        public List<VisualizarProdutoViewModel> ProdutosMaisVendidos()
        {
            List<VisualizarProdutoViewModel>? vms = _dbContext.Produtos.Include(x => x.Categoria).ToList().Select(produto => new VisualizarProdutoViewModel()
            {
                Id = produto.Id,
                Nome= produto.Nome,
                Preco = produto.Preco,
                QuantidadeVenda= produto.QuantidadeVenda,
                Categoria = new VisualizarCategoriaProdutoViewModel()
                {
                    Id = produto.Id,
                    Nome = produto.Nome
                }
            }).OrderByDescending(produto=> produto.QuantidadeVenda).Take(5).ToList();

            return vms;
        }

        public TotalVendidoProdutoViewModel totalVendas()
        {
            TotalVendidoProdutoViewModel totalVendas = new TotalVendidoProdutoViewModel()
            {
                TotalVendido = _dbContext.Produtos.Sum(produto => produto.QuantidadeVenda)
            };
            
            return totalVendas;
        }

    }
}
