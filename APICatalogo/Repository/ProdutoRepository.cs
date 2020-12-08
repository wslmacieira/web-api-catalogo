﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {

        }

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {

            return PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId), 
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        public async Task<IEnumerable<Produto>> GetProdutoPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }
    }
}
