
using MercadoLivre_API.Data;
using MercadoLivre_API.Exceptions;
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
        [HttpGet("offset/{offset:int}/limit/{limit:int}")]
        public ActionResult GetCategorias([FromRoute] int offset, [FromRoute] int limit)
        {
            try
            {
                List<VisualizarCategoriaViewModel> categorias = _categoriaService.VisualizarCategorias(offset,limit);

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

                return Ok(new ResultViewModel<VisualizarCategoriaViewModel>(vm));
            }
            catch(NotFoundException ex)
            {
                return NotFound(new ResultViewModel<VisualizarCategoriaViewModel>(ex.Message));
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
            catch (NotFoundException ex)
            {
                return NotFound(new ResultViewModel<VisualizarCategoriaViewModel>(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x03 - Falha interna no servidor"));
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
            catch (NotFoundException ex)
            {
                return NotFound(new ResultViewModel<VisualizarCategoriaViewModel>(ex.Message));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>("01x04 - Não foi possível alterar a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>(ex.Message));
            }
        }

        [HttpDelete("{idCategoria:int}")]
        public ActionResult DeleteCategoriaAsync([FromRoute] int idCategoria)
        {
            try
            {
                _categoriaService.DeletarCategoria(idCategoria);

                return Ok("Categoria removida com sucesso");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResultViewModel<VisualizarCategoriaViewModel>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<VisualizarCategoriaViewModel>(ex.Message));
            }
        }
    }
}
