using MercadoLivre_API.Data;
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

        public List<VisualizarProdutoViewModel> VisualizarProdutos()
        {
            try
            {
                List<Produto>? produtos = _dbContext.Produtos.Include(x => x.Categoria).ToList();

                if (!produtos.Any())
                {
                    throw new Exception("Não há produtos cadastrados");
                }

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
            catch (Exception)
            {
                throw new Exception("Erro ao consultar os produtos");
            }
        }

        public VisualizarProdutoViewModel VisualizarProduto(int id)
        {
            try
            {
                Produto? produto = _dbContext.Produtos.Include(x => x.Categoria).SingleOrDefault(produto => produto.Id == id);

                if (produto == null)
                {
                    throw new Exception("Produto não localizado");
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
            catch (Exception)
            {
                throw new Exception("Erro ao consultar o produto");
            }
        }

        public int CadastrarProduto(PostPutProdutoViewModel vm)
        {
            try
            {
                Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == vm.IdCategoria);

                if (categoria == null)
                    throw new Exception("Categoria não encontrada");

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
            catch (Exception)
            {
                throw new Exception("Erro ao cadastrar produto");
            }
        }

        public VisualizarProdutoViewModel EditarProduto(int idProduto, PostPutProdutoViewModel vm)
        {
            try
            {
                Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == vm.IdCategoria);

                if (categoria == null)
                    throw new Exception("Categoria não encontrada");

                Produto? produto = _dbContext.Produtos.SingleOrDefault(produto => produto.Id == idProduto);

                if (produto == null)
                    throw new Exception("Produto não encontrado");

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
            catch (Exception)
            {
                throw new Exception("Erro ao editar produto");
            }
        }

        public void DeletarProduto(int idProduto)
        {
            try
            {
                Produto? produto = _dbContext.Produtos.SingleOrDefault(produto => produto.Id == idProduto);

                if (produto == null)
                    throw new Exception("Produto não encontrado");

                _dbContext.Produtos.Remove(produto);

                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Erro ao remover produto");
            }
        }

    }
}
