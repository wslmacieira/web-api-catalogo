using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public CategoriasController(AppDbContext contexto, IConfiguration config, ILogger<CategoriasController> logger)
        {
            _context = contexto;
            _configuration = config;
            _logger = logger;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["autor"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];

            return $"Autor : {autor} Conexão: {conexao}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico,
            string nome)
        {
            return meuservico.Saudacao(nome);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("========= GET api/categorias/produtos =========");
            return _context.Categorias.Include(x => x.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                _logger.LogInformation("========= GET api/categorias =========");
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch (System.Exception)
            {

                return StatusCode(500, "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {

                var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
                _logger.LogInformation($"========= GET api/categorias/id = {id} =========");

                if (categoria == null)
                {
                    _logger.LogInformation($"========= GET api/categorias/id = {id} NOT FOUND =========");
                    return NotFound($"A categoria com o id={id} não foi encontrada");
                }
                return categoria;
            }
            catch (System.Exception)
            {

                return StatusCode(500, "Erro ao tentar obter a categoria do banco de dados");
            }


        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);
            }
            catch (System.Exception)
            {

                return StatusCode(500, "Erro ao tentar criar uma nova categoria");
            }

        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com o id={id}");
                }

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok($"Categoria com o id={id} foi atualizada com sucesso");
            }
            catch (System.Exception)
            {

                return StatusCode(500, $"Erro ao tentar atualizar categoria com id={id}");
            }

        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                _context.Remove(categoria);
                _context.SaveChanges();
                return categoria;
            }
            catch (System.Exception)
            {

                return StatusCode(500, $"Erro ao excluir a categoria de id={id}");
            }

        }
    }
}
