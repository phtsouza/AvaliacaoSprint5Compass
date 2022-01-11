/*
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

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
                if (ContemCidade != null)
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

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Cliente> Clientes = _context.Clientes.Include(C => C.cidade);
            var ClientesDTO = _mapper.Map<IEnumerable<ClienteDTO>>(Clientes);
            return Ok(ClientesDTO);
        }

        [HttpGet("{Id}")]
        public IActionResult GetId(Guid Id)
        {
            Cliente ClienteProcurado = _context.Clientes.Include(C => C.cidade).FirstOrDefault(Cl => Cl.Id == Id);
            
            if(ClienteProcurado != null)
            {
                ClienteDTO ClienteRetornado = _mapper.Map<ClienteDTO>(ClienteProcurado);
                return Ok(ClienteRetornado);
            }
            return NotFound("Cliente não cadastrado no banco de dados");
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            Cliente ClienteProcurado = _context.Clientes.FirstOrDefault(Cl => Cl.Id == Id);

            if (ClienteProcurado != null)
            {
                _context.Remove(ClienteProcurado);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound("Cliente não cadastrado no banco de dados");
        }

        [HttpPut("{Id}")]
        public IActionResult AtualizaId(Guid Id, [FromBody] ClienteDTO clienteDTO)
        {
            Cliente ClienteProcurado = _context.Clientes.FirstOrDefault(Cl => Cl.Id == Id);

            if (ClienteProcurado != null)
            {
                var cepOriginal = ClienteProcurado.Cep;

                _mapper.Map(clienteDTO, ClienteProcurado);

                if(cepOriginal != clienteDTO.Cep)
                {
                    ViaCepDTO viaCepDTO = BuscaCep(clienteDTO.Cep);

                    ClienteProcurado.Logradouro = viaCepDTO.logradouro;

                    ClienteProcurado.Bairro = viaCepDTO.bairro;
                }

                _context.SaveChanges();
                return NoContent();
            }
            return NotFound("Cliente não cadastrado no banco de dados");
        }

    }
}
*/