using MercadoLivre_API.Data;
using MercadoLivre_API.Exceptions;
using MercadoLivre_API.Models;
using MercadoLivre_API.Services;
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
    private readonly ProdutoService _produtoService;


    public ProdutoController(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet("offset/{offset:int}/limit/{limit:int}")]
    public ActionResult<Produto> GetProdutos([FromRoute] int offset, [FromRoute] int limit)
    {
        try
        {
            List<VisualizarProdutoViewModel> vms = _produtoService.VisualizarProdutos(offset,limit);

            return Ok(new ResultViewModel<List<VisualizarProdutoViewModel>>(vms));

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<VisualizarProdutoViewModel>(ex.Message));
        }
    }

    [HttpGet("{idProduto:int}")]
    public ActionResult GetProduto([FromRoute] int idProduto)
    {
        try
        {
            VisualizarProdutoViewModel vm = _produtoService.VisualizarProduto(idProduto);

            return Ok(new ResultViewModel<VisualizarProdutoViewModel>(vm));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ResultViewModel<VisualizarProdutoViewModel>(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Produto>(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostProdutoAsync([FromBody] PostPutProdutoViewModel vm)
    {
        try
        {
            int idProduto = _produtoService.CadastrarProduto(vm);

            return Created($"v1/produtos/{idProduto}", new ResultViewModel<PostPutProdutoViewModel>(vm));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ResultViewModel<VisualizarProdutoViewModel>(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Categoria>(ex.Message));
        }
    }

    [HttpPut("{idProduto:int}")]
    public async Task<IActionResult> PutProdutoAsync([FromRoute] int idProduto, [FromBody] PostPutProdutoViewModel vm)
    {
        try
        {
            VisualizarProdutoViewModel vmRetorno = _produtoService.EditarProduto(idProduto, vm);

            return Ok(new ResultViewModel<VisualizarProdutoViewModel>(vmRetorno));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ResultViewModel<VisualizarProdutoViewModel>(ex.Message));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Produto>("02x04 - Não foi possível alterar o produto"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Produto>(ex.Message));
        }
    }

    [HttpDelete("{idProduto:int}")]
    public async Task<IActionResult> DeleteProdutoAsync([FromRoute] int idProduto)
    {
        try
        {
            _produtoService.DeletarProduto(idProduto);

            return Ok("Produto removido com sucesso");
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ResultViewModel<VisualizarProdutoViewModel>(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<VisualizarProdutoViewModel>(ex.Message));
        }
    }

    [HttpGet("maisvendidos")]
    public async Task<IActionResult> GetMaisVendidos()
    {
        try
        {
            List<VisualizarProdutoViewModel>? vms = _produtoService.ProdutosMaisVendidos();

            return Ok(new ResultViewModel<List<VisualizarProdutoViewModel>>(vms));
        }
        catch (Exception ex )
        {
            return StatusCode(500, new ResultViewModel<VisualizarProdutoViewModel>(ex.Message));
        }
    }

    [HttpGet("quantidadetotalvendida")]
    public async Task<IActionResult> GetTotalVendido()
    {
        try
        {
            TotalVendidoProdutoViewModel totalVenda = _produtoService.totalVendas();

            return Ok(new ResultViewModel<TotalVendidoProdutoViewModel>(totalVenda));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<TotalVendidoProdutoViewModel>(ex.Message));
        }
    }

};