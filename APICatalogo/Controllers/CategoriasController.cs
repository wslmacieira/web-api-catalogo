using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>>
            Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDto;
            }
            catch (System.Exception)
            {

                return StatusCode(500, "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com o id={id} não foi encontrada");
                }

                var categoriasDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriasDto;
            }
            catch (System.Exception)
            {

                return StatusCode(500, "Erro ao tentar obter a categoria do banco de dados");
            }


        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (System.Exception)
            {

                return StatusCode(500, "Erro ao tentar criar uma nova categoria");
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com o id={id}");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();
                return Ok($"Categoria com o id={id} foi atualizada com sucesso");
            }
            catch (System.Exception)
            {

                return StatusCode(500, $"Erro ao tentar atualizar categoria com id={id}");
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

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
