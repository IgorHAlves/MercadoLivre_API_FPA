
using MercadoLivre_API.Data;
using MercadoLivre_API.Models;
using MercadoLivre_API.ViewModels;
using MercadoLivre_API.ViewModels.CategoriaViewModel;
using MercadoLivre_API.ViewModels.ProdutoViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MercadoLivre_API.Controllers
{
    [ApiController]
    [Route("v1/categorias")]

    public class CategoriaController : ControllerBase
    {
        private readonly MercadoLivreDataContext _dbContext;
        public CategoriaController([FromServices] MercadoLivreDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetCategoriasAsync()
        {
            try
            {
                List<Categoria> categorias = await _dbContext.Categorias.Include(x => x.Produtos).ToListAsync();
                if (categorias.Count == 0)
                    return NotFound(new ResultViewModel<Categoria>("Não existem categorias cadastradas"));

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

                return Ok(new ResultViewModel<List<VisualizarCategoriaViewModel>>(vms));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x01 - Falha interna no servidor"));
            }
        }

        [HttpGet("{idCategoria:int}")]
        public async Task<IActionResult> GetCategoriaAsync([FromRoute] int idCategoria)
        {
            try
            {
                Categoria categoria = await _dbContext.Categorias.Include(x => x.Produtos).FirstOrDefaultAsync(cat => cat.Id == idCategoria) ?? new Categoria();

                if (categoria.Id == 0)
                    return NotFound(new ResultViewModel<Categoria>("Categoria não encontrada"));

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

                return Ok(new ResultViewModel<VisualizarCategoriaViewModel>(vm));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x02 - Falha interna no servidor"));

            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostCategoriaAsync([FromBody] PostPutCategoriaViewModel vm)
        {
            try
            {
                List<Produto> produtos = await _dbContext.Produtos.Where(produto => vm.IdProdutos.Contains(produto.Id)).ToListAsync();

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

                await _dbContext.Categorias.AddAsync(categoria);

                await _dbContext.SaveChangesAsync();

                return Created($"$v1/categorias/{categoria.Id}", new ResultViewModel<Categoria>(categoria));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Categoria>("01x03 - Falha interna no servidor"));
            }
        }

        [HttpPut("{idCategoria:int}")]
        public async Task<IActionResult> PutCategoriaAsync([FromRoute] long idCategoria, [FromBody] PostPutCategoriaViewModel vm)
        {
            try
            {
                List<Produto> produtos = await _dbContext.Produtos.Where(produto => vm.IdProdutos.Contains(produto.Id)).ToListAsync();

                Categoria? categoria = await _dbContext.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == idCategoria);

                if (categoria == null)
                {
                    return NotFound("Categoria não localizada");
                }

                categoria.Nome = vm.Nome;

                if (produtos.Count != 0)
                    categoria.Produtos = produtos;

                await _dbContext.SaveChangesAsync();

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

                return Ok(new ResultViewModel<VisualizarCategoriaViewModel>(vmRetorno));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x04 - Não foi possível alterar a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x05 - Falha interna no servidor"));
            }
        }

        [HttpDelete("{idCategoria:int}")]
        public async Task<IActionResult> DeleteCategoriaAsync([FromRoute] int idCategoria)
        {
            try
            {
                Categoria categoria = await _dbContext.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == idCategoria) ?? new Categoria();

                if (categoria.Id == 0)
                    return NotFound("Categoria não encontrada");

                _dbContext.Categorias.Remove(categoria);
                await _dbContext.SaveChangesAsync();

                return Ok("Categoria removida com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x06 - Falha interna no servidor"));
            }
        }

    }
}
