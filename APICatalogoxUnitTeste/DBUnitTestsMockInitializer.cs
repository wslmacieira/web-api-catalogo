using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogoxUnitTeste
{
    public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer()
        { }

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add
                (new Categoria { CategoriaId = 999, Nome = "Bebidas999", ImagemUrl = "bebidas999.jpg" });
            context.Categorias.Add
               (new Categoria { CategoriaId = 2, Nome = "Sucos", ImagemUrl = "sucos.jpg" });
            context.Categorias.Add
               (new Categoria { CategoriaId = 3, Nome = "Doces", ImagemUrl = "doces.jpg" });
            context.Categorias.Add
               (new Categoria { CategoriaId = 4, Nome = "Salgados", ImagemUrl = "salgados.jpg" });
            context.Categorias.Add
               (new Categoria { CategoriaId = 5, Nome = "Tortas", ImagemUrl = "tortas.jpg" });
            context.Categorias.Add
               (new Categoria { CategoriaId = 6, Nome = "Bolos", ImagemUrl = "bolos.jpg" });
            context.Categorias.Add
               (new Categoria { CategoriaId = 7, Nome = "Lanches", ImagemUrl = "lanches.jpg" });

            context.SaveChanges();
        }
    }
}
