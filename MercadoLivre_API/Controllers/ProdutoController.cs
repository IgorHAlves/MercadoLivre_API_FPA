using MercadoLivre_API.Data;
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

    [HttpGet]
    public ActionResult<Produto> GetProdutos()
    {
        try
        {
            List<VisualizarProdutoViewModel> vms = _produtoService.VisualizarProdutos();

            if (vms.Count == 0)
                return NotFound("Não há produtos cadastrados");

            return Ok(new ResultViewModel<List<VisualizarProdutoViewModel>>(vms));

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<VisualizarProdutoViewModel>("02x01 - Falha interna no servidor"));
        }
    }

    [HttpGet("{idProduto:int}")]
    public ActionResult GetProduto([FromRoute] int idProduto)
    {
        try
        {
            VisualizarProdutoViewModel vm = _produtoService.VisualizarProduto(idProduto);

            if (vm == null)
                return NotFound("Produto não localizado");

            return Ok(new ResultViewModel<VisualizarProdutoViewModel>(vm));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Produto>("02x02 - Falha interna no servidor"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostProdutoAsync([FromBody] PostPutProdutoViewModel vm)
    {
        try
        {
            int idProduto = _produtoService.CadastrarProduto(vm);

            if (idProduto == 0)
                return BadRequest("Não foi possível cadastrar o produto enviado");

            return Created($"v1/produtos/{idProduto}", new ResultViewModel<PostPutProdutoViewModel>(vm));
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
            VisualizarProdutoViewModel vmRetorno = _produtoService.EditarProduto(idProduto, vm);

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
            _produtoService.DeletarProduto(idProduto);

            return Ok("Produto removido com sucesso");
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<VisualizarProdutoViewModel>("02x06 - Falha interna no servidor"));
        }
    }
};