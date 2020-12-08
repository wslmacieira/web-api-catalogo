using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutoPorPreco();
    }
}
