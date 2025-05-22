using MercadoLivre_API.Data;
using MercadoLivre_API.Models;
using MercadoLivre_API.ViewModels.CategoriaViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MercadoLivre_API.Services
{
    public class CategoriaService
    {
        private readonly MercadoLivreDataContext _dbContext;

        public CategoriaService([FromServices] MercadoLivreDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<VisualizarCategoriaViewModel> VisualizarCategorias()
        {
            try
            {
                List<Categoria> categorias = _dbContext.Categorias.Include(x => x.Produtos).ToList();
                if (categorias.Count != 0)
                {

                    List<VisualizarCategoriaViewModel> vms = categorias.Select(categoria => new VisualizarCategoriaViewModel
                    {
                        Id = categoria.Id,
                        Nome = categoria.Nome,
                        Produtos = categoria.Produtos.Select(produto => new VisualizarProdutoCategoriaViewModel
                        {
                            Id = produto.Id,
                            Nome = produto.Nome
                        }).ToList()
                    }
                    ).ToList();

                    return vms;
                }

                return new List<VisualizarCategoriaViewModel>();

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar categorias:{ex.Message}");
            }
        }

        public VisualizarCategoriaViewModel VisualizarCategoria(int idCategoria)
        {
            try
            {
                Categoria categoria = _dbContext.Categorias.Include(x => x.Produtos).SingleOrDefault(cat => cat.Id == idCategoria) ?? new Categoria();

                if (categoria.Id != 0)
                {
                    VisualizarCategoriaViewModel vm = new VisualizarCategoriaViewModel
                    {
                        Id = categoria.Id,
                        Nome = categoria.Nome,
                        Produtos = categoria.Produtos.Select(produto => new VisualizarProdutoCategoriaViewModel
                        {
                            Id = produto.Id,
                            Nome = produto.Nome
                        }).ToList()
                    };

                    return vm;
                }

                return new VisualizarCategoriaViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar categoria:{ex.Message}");
            }
        }

        public int InserirCategoria(PostPutCategoriaViewModel vm)
        {
            try
            {
                List<Produto> produtos = _dbContext.Produtos.Where(produto => vm.IdProdutos.Contains(produto.Id)).ToList();

                Categoria categoria = new Categoria
                {
                    Nome = vm.Nome
                };

                if (produtos.Count != 0)
                {
                    foreach (Produto produto in produtos)
                    {
                        categoria.Produtos.Add(produto);
                    }
                }

                _dbContext.Categorias.AddAsync(categoria);

                _dbContext.SaveChanges();

                return categoria.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar categoria:{ex.Message}");
            }
        }
        public VisualizarCategoriaViewModel EditarCategoria(int idCategoria, PostPutCategoriaViewModel vm)
        {
            try
            {
                Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == idCategoria);

                if (categoria == null)
                {
                    throw new Exception($"Categoria não localizada");
                }

                List<Produto>? produtos = _dbContext.Produtos.Where(produto => vm.IdProdutos.Contains(produto.Id)).ToList();

                if (produtos.Count != 0)
                    categoria.Produtos = produtos;

                categoria.Nome = vm.Nome;

                _dbContext.SaveChanges();

                VisualizarCategoriaViewModel vmRetorno = new VisualizarCategoriaViewModel
                {
                    Id = categoria.Id,
                    Nome = categoria.Nome,
                    Produtos = categoria.Produtos.Select(produto => new VisualizarProdutoCategoriaViewModel
                    {
                        Id = produto.Id,
                        Nome = produto.Nome
                    }).ToList()
                };

                return vmRetorno;
            }

            catch (Exception)
            {
                throw;
            }
        }
        
        public void DeletarCategoria(int idCategoria)
        {
            try
            {
                Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == idCategoria);

                if (categoria == null)
                {
                    throw new Exception("Categoria não localizada");
                }
                _dbContext.Categorias.Remove(categoria);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
