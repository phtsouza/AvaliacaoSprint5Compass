using CidadesClientes_API.Context;
using CidadesClientes_API.DTOS;
using CidadesClientes_API.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CidadesClientes_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private ClienteCidadeDbContext _context;
        private IMapper _mapper;
        
        public CidadeController(ClienteCidadeDbContext Contexto, IMapper mapper) 
        {
            _context = Contexto;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CidadeDTO cidadeDTO)
        {
            var NovaCidade = VerificaIgualdade(cidadeDTO.Nome, cidadeDTO.Estado);
            
            if(NovaCidade == null)
            {
                Cidade cidade = _mapper.Map<Cidade>(cidadeDTO);
                _context.Cidades.Add(cidade);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest(new { Mensagem = "Cidade já existente no banco de dados", Erro = true });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public CidadeDTO VerificaIgualdade(string nomeCidade, string estadoCidade)
        {
            var ContemCidade = _context.Cidades.FirstOrDefault(C => C.Nome == nomeCidade && C.Estado == estadoCidade);

            if(ContemCidade != null)
            {
                CidadeDTO cidadeRetorno = _mapper.Map<CidadeDTO>(ContemCidade);
                return cidadeRetorno;
            }
            return null;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Cidade> TodasCidades = _context.Cidades;

            var ListaCidadesDTO = _mapper.Map<IEnumerable<CidadeDTO>>(TodasCidades);

            return Ok(ListaCidadesDTO);
        }
    }
}
