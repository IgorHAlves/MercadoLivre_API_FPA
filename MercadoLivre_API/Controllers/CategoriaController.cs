
using MercadoLivre_API.Data;
using MercadoLivre_API.Models;
using MercadoLivre_API.Services;
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
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [HttpGet("")]
        public ActionResult GetCategorias()
        {
            try
            {
                List<VisualizarCategoriaViewModel> categorias = _categoriaService.VisualizarCategorias();

                if (categorias.Count == 0)
                    return NotFound(new ResultViewModel<Categoria>("Não existem categorias cadastradas"));

                return Ok(new ResultViewModel<List<VisualizarCategoriaViewModel>>(categorias));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x01 - Falha interna no servidor"));
            }
        }

        [HttpGet("{idCategoria:int}")]
        public ActionResult GetCategoria([FromRoute] int idCategoria)
        {
            try
            {
                VisualizarCategoriaViewModel vm = _categoriaService.VisualizarCategoria(idCategoria);

                if (vm.Id == 0)
                    return NotFound(new ResultViewModel<VisualizarCategoriaViewModel>("Categoria não encontrada"));

                return Ok(new ResultViewModel<VisualizarCategoriaViewModel>(vm));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x02 - Falha interna no servidor"));

            }
        }

        [HttpPost("")]
        public ActionResult PostCategoriaAsync([FromBody] PostPutCategoriaViewModel vm)
        {
            try
            {
                long idCategoria = _categoriaService.InserirCategoria(vm);

                return Created($"$v1/categorias/{idCategoria}", new ResultViewModel<PostPutCategoriaViewModel>(vm));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Categoria>("01x03 - Falha interna no servidor"));
            }
        }

        [HttpPut("{idCategoria:int}")]
        public ActionResult PutCategoriaAsync([FromRoute] int idCategoria, [FromBody] PostPutCategoriaViewModel vm)
        {
            try
            {
                VisualizarCategoriaViewModel vmRetorno = _categoriaService.EditarCategoria(idCategoria, vm);

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
                _categoriaService.DeletarCategoria(idCategoria);

                return Ok("Categoria removida com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x06 - Falha interna no servidor"));
            }
        }
    }
}
