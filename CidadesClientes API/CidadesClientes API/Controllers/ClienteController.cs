using AutoMapper;
using CidadesClientes_API.Context;
using CidadesClientes_API.DTOS;
using CidadesClientes_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CidadesClientes_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private ClienteCidadeDbContext _context;
        private IMapper _mapper;

        public ClienteController(ClienteCidadeDbContext Contexto, IMapper mapper)
        {
            _context = Contexto;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClienteDTO clienteDTO)
        {
            ViaCepDTO viaCepDTO = BuscaCep(clienteDTO.Cep);

            if (viaCepDTO != null)
            {
                Cliente cliente = _mapper.Map<Cliente>(clienteDTO);

                cliente.Logradouro = viaCepDTO.logradouro;

                cliente.Bairro = viaCepDTO.bairro;
 
                var ContemCidade = _context.Cidades.FirstOrDefault(C => C.Nome == viaCepDTO.localidade && C.Estado == viaCepDTO.uf);
                if(ContemCidade != null)
                {
                    cliente.CidadeId = ContemCidade.Id;

                    _context.Clientes.Add(cliente);
                    _context.SaveChanges();
                    return Ok("Cliente adicionado com sucesso");
                }
                
                return BadRequest("Cidade inexistente no banco, primeiro adicione uma cidade para depois adicionar um cliente");
                
                 
            }
            return NotFound("Não foi possível encontrar o cep desejado");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ViaCepDTO BuscaCep(string CEP)
        {
            var Url = $"http://www.viacep.com.br/ws/{CEP}/json/";
            var Requisicao = WebRequest.Create(Url);
            Requisicao.Method = "GET";

            using var Resposta = Requisicao.GetResponse();
            using var Stream = Resposta.GetResponseStream();
            using var Leitor = new StreamReader(Stream);

            var JsonViaCep = Leitor.ReadToEnd();
            var result = JsonViaCep;
            var ViaCepData = JsonConvert.DeserializeObject<ViaCepDTO>(result);

            return ViaCepData;
        }

    }
}