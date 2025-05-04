using MercadoLivre_API.Data;
using MercadoLivre_API.Models;
using MercadoLivre_API.ViewModels;
using MercadoLivre_API.ViewModels.CategoriaViewModel;
using MercadoLivre_API.ViewModels.ProdutoViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;

namespace MercadoLivre_API.Controllers;

[ApiController]
[Route("v1/produtos")]
public class ProdutoController : ControllerBase
{
    private readonly MercadoLivreDataContext _dbContext;

    public ProdutoController(MercadoLivreDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<Produto>> GetProdutosAsync()
    {
        try
        {
            List<Produto> produtos = await _dbContext.Produtos.Include(x => x.Categoria).ToListAsync();

            if (produtos.Count == 0)
                return NotFound("Não há produtos cadastrados");

            List<VisualizarProdutoViewModel> vms = new List<VisualizarProdutoViewModel>();

            foreach(Produto produto in produtos)
            {
                vms.Add(new VisualizarProdutoViewModel()
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
                });
            }

            return Ok(new ResultViewModel<List<VisualizarProdutoViewModel>>(vms));

        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<VisualizarProdutoViewModel>("02x01 - Falha interna no servidor"));

        }
    }

    [HttpGet("{idProduto:int}")]
    public async Task<IActionResult> GetProdutoAsync([FromRoute] int idProduto)
    {
        try
        {
            Produto produto = await _dbContext.Produtos.Include(x => x.Categoria).FirstOrDefaultAsync(produto => produto.Id == idProduto) ?? new Produto();

            if (produto.Id == 0)
                return NotFound("Produto não encontrado");

            VisualizarProdutoViewModel vm = new VisualizarProdutoViewModel()
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
            };

            return Ok(new ResultViewModel<VisualizarProdutoViewModel>(vm));
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<Produto>("02x02 - Falha interna no servidor"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostProdutoAsync([FromBody] PostPutProdutoViewModel vm)
    {
        try
        {
            Produto produto = new Produto
            {
                Nome = vm.Nome,
                Preco = vm.Preco,
                QuantidadeVenda = vm.QuantidadeVenda
            };

            Categoria categoria = await _dbContext.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == vm.IdCategoria) ?? new Categoria();

            if (categoria.Id != 0)
                produto.Categoria = categoria;

            await _dbContext.Produtos.AddAsync(produto);

            VisualizarProdutoViewModel vmRetorno = new VisualizarProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeVenda = produto.QuantidadeVenda,
                Categoria = new VisualizarCategoriaProdutoViewModel
                {
                    Id = produto.Categoria.Id,
                    Nome = produto.Categoria.Nome
                }
            };

            await _dbContext.SaveChangesAsync();

            return Created($"v1/produtos/{produto.Id}", new ResultViewModel<VisualizarProdutoViewModel>(vmRetorno));

        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Categoria>("02x03 - Falha interna no servidor"));
        }
    }

    [HttpPut("{idProduto:int}")]
    public async Task<IActionResult> PutProdutoAsync([FromRoute] int idProduto, [FromBody] PostPutProdutoViewModel vm)
    {
        try
        {
            Produto? produto = await _dbContext.Produtos.Include(categoria => categoria.Categoria).FirstOrDefaultAsync(produto => produto.Id == idProduto);

            if (produto == null)
                return NotFound("Produto não encontrado");

            produto.Nome = vm.Nome;
            produto.Preco = vm.Preco;
            produto.QuantidadeVenda = vm.QuantidadeVenda;

            Categoria? categoria = await _dbContext.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == vm.IdCategoria);

            if (categoria == null)
                return NotFound("Categoria não localizada");

            if (categoria.Id != produto.Categoria.Id)
            {
                produto.Categoria = categoria;
            }

            await _dbContext.SaveChangesAsync();

            VisualizarProdutoViewModel vmRetorno = new VisualizarProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeVenda = produto.QuantidadeVenda,
                Categoria = new VisualizarCategoriaProdutoViewModel
                {
                    Id = produto.Categoria.Id,
                    Nome = produto.Categoria.Nome
                }
            };

            return Ok(new ResultViewModel<VisualizarProdutoViewModel>(vmRetorno));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Produto>("02x04 - Não foi possível alterar o produto"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Produto>("02x05 - Falha interna no servidor"));
        }
    }

    [HttpDelete("{idProduto:int}")]
    public async Task<IActionResult> DeleteProdutoAsync([FromRoute] int idProduto)
    {
        try
        {
            Produto? produto = await _dbContext.Produtos.FirstOrDefaultAsync(produto => produto.Id == idProduto);

            if (produto == null)
                return NotFound("Produto não encontrado");

            _dbContext.Produtos.Remove(produto);
            await _dbContext.SaveChangesAsync();

            return Ok("Produto removido com sucesso");
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<VisualizarProdutoViewModel>("02x06 - Falha interna no servidor"));
        }
    }
};