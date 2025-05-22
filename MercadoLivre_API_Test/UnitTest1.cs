using MercadoLivre_API.Data;
using MercadoLivre_API.Models;
using MercadoLivre_API.Services;
using MercadoLivre_API.ViewModels.CategoriaViewModel;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace MercadoLivre_API_Test
{
    public class UnitTest1
    {
        private MercadoLivreDataContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MercadoLivreDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new MercadoLivreDataContext(options);
            databaseContext.Database.EnsureCreated();
            //if(databaseContext.Categorias.Count() < 0 )
            //{
            //    databaseContext.Categorias.Add(
            //        new Categoria
            //        {
            //            Id = 1,
            //            Nome = "Eletrodomesticos"
            //        });

            //    databaseContext.SaveChanges();
            //}

            return databaseContext;
        }

        [Fact]
        public void Test1()
        {
            MercadoLivreDataContext dbContext = GetDbContext();

            CategoriaService categoriaService = new CategoriaService(dbContext);

            //Act
            int retorno = categoriaService.InserirCategoria(new PostPutCategoriaViewModel
            {
                Nome = "Smartphones"
            });

            //Assert
            retorno.ShouldBe(retorno);
        }
    }
}
