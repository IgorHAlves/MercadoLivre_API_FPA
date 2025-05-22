using MercadoLivre_API.Data;
using MercadoLivre_API.Exceptions;
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
            List<Categoria> categorias = _dbContext.Categorias.Include(x => x.Produtos).ToList();

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

        public VisualizarCategoriaViewModel VisualizarCategoria(int idCategoria)
        {

            Categoria? categoria = _dbContext.Categorias
                .Include(x => x.Produtos)
                .SingleOrDefault(cat => cat.Id == idCategoria);

            if (categoria == null)
                throw new NotFoundException("Categoria não encontrada");

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

        public int InserirCategoria(PostPutCategoriaViewModel vm)
        {

            List<Produto> produtos = _dbContext.Produtos.Where(produto => vm.IdProdutos.Contains(produto.Id)).ToList();

            Categoria categoria = new Categoria
            {
                Nome = vm.Nome
            };

            if (produtos.Count < vm.IdProdutos.Count)
            {
                throw new NotFoundException("Há produtos não cadastrados na lista de produtos");
            }

            foreach (Produto produto in produtos)
            {
                categoria.Produtos.Add(produto);
            }

            _dbContext.Categorias.AddAsync(categoria);

            _dbContext.SaveChanges();

            return categoria.Id;
        }
        public VisualizarCategoriaViewModel EditarCategoria(int idCategoria, PostPutCategoriaViewModel vm)
        {
            Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == idCategoria);

            if (categoria == null)
            {
                throw new NotFoundException($"Categoria não localizada");
            }

            List<Produto>? produtos = _dbContext.Produtos.Where(produto => vm.IdProdutos.Contains(produto.Id)).ToList();

            if (vm.IdProdutos!.Count != produtos.Count)
                throw new NotFoundException("Um ou mais produtos informados não foram encontrados.");

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

        public void DeletarCategoria(int idCategoria)
        {
            Categoria? categoria = _dbContext.Categorias.SingleOrDefault(categoria => categoria.Id == idCategoria);

            if (categoria == null)
            {
                throw new NotFoundException("Categoria não localizada");
            }
            _dbContext.Categorias.Remove(categoria);
            _dbContext.SaveChanges();
        }

    }
}
