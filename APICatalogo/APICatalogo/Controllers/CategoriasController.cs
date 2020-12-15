using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork contexto, IMapper mapper)
        {
            _context = contexto;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém os produtos relacionados para cada categoria
        /// </summary>
        /// <returns>Objetos Categoria e respectivo Objetos Produtos</returns>
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            var categorias = _context.CategoriaRepository.GetCategoriasProdutos();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;
        }

        /// <summary>
        /// Retorna uma coleção de objetos Categoria
        /// </summary>
        /// <returns>Lista de Categorias</returns>
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            try
            {
                var categorias = _context.CategoriaRepository.Get().ToList();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("paginacao")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetPaginacao(int pag = 1, int reg = 5)
        {
            try
            {
                if (reg > 99)
                    reg = 5;

                var categorias = _context.CategoriaRepository
                .LocalizaPagina<Categoria>(pag, reg)
                .ToList();

                var totalRegistros = _context.CategoriaRepository.getTotalRegistros();
                var numeroPaginas = ((int)Math.Ceiling((double)totalRegistros / reg));

                Response.Headers["X-Total-Registros"] = totalRegistros.ToString();
                Response.Headers["X-Numero-Paginas"] = numeroPaginas.ToString();

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        /// <summary>
        /// Obtem uma Categoria pelo seu Id
        /// </summary>
        /// <param name="id">codigo da categoria</param>
        /// <returns>Objeto Categoria</returns>
        [HttpGet("{id}", Name = "ObterCategoria")]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoriaDTO> Get(int id)
        {

            var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDto;

        }

        /// <summary>
        /// Inclui uma nova Categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/categorias
        ///     {
        ///         "categoriaId": 1,
        ///         "nome": "categoria1",
        ///         "imagemUrl": "http://teste.net/1.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDto">objeto Categoria</param>
        /// <returns>O objeto Categoria incluida</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _context.CategoriaRepository.Add(categoria);
            _context.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoriaDTO);
        }

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Put))]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest($"Não foi possível atualizar a categoria com o id={id}");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _context.CategoriaRepository.Update(categoria);
            _context.Commit();
            return Ok($"Categoria com o id={id} foi atualizada com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var categoria = await _context.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                _context.CategoriaRepository.Delete(categoria);
                await _context.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch (System.Exception)
            {

                return StatusCode(500, $"Erro ao excluir a categoria de id={id}");
            }

        }
    }
}
