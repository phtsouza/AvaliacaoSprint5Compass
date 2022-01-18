using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.DTOS.ClienteDTOS;
using CidadesClientesServices.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CidadesClientes_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private IClienteServices _clienteService;
        private ICidadeServices _cidadeServices;

        public ClienteController(IClienteServices clienteServices, ICidadeServices cidadeServices)
        {
            _clienteService = clienteServices;
            _cidadeServices = cidadeServices;
        }

        // Método para adicionar um cliente no banco de dados
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post([FromBody] ClienteDTO clienteDTO)
        {
            var result = _clienteService.VerificaErros(clienteDTO); // Faz a verificação das informações do cliente a ser adicionado
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros); // Caso tenha algum erro, retorna BadRequest
            }

            ViaCepDTO viaCepDTO = _clienteService.BuscaCep(clienteDTO.Cep); // Faz a busca do cep, pela API ViaCep

            if (viaCepDTO.localidade != null)
            {
                ClienteRetornaDTO clienteNovo = _clienteService.Cadastra(clienteDTO, viaCepDTO); 

                if(clienteNovo == null) // Caso ele tente cadastrar sem a cidade existir
                {
                    CidadeDTO novaCidade = new CidadeDTO();
                    novaCidade.Nome = viaCepDTO.localidade;
                    novaCidade.Estado = viaCepDTO.uf;
                    _cidadeServices.CadastrarCidade(novaCidade); // A cidade primeiro vai ser cadastrada
                }
                else
                {
                    return CreatedAtAction(nameof(GetId), new { Id = clienteNovo.Id }, clienteNovo); // Caso a cidade já exista, apenas é retornado o Id do novo cliente
                }

                clienteNovo = _clienteService.Cadastra(clienteDTO, viaCepDTO); // Para depois o cliente ser cadastrado
                return CreatedAtAction(nameof(GetId), new { Id = clienteNovo.Id }, clienteNovo); // Ao final retorna o Id do novo Cliente
            }
            return NotFound("Não foi possível encontrar o cep desejado"); // Caso o cep não seja encontrado no ViaCep, retorna NotFound
        }
        
        // Método pelo qual retorna todos os clientes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_clienteService.GetAll());
        }
        
        // Método pelo qual retorna o cliente pelo seu Id
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetId(Guid Id)
        {
            
            Cliente ClienteProcurado = _clienteService.ProcuraCliente(Id); // procura o Cliente pelo seu Id

            if(ClienteProcurado != null)
            {
                return Ok(_clienteService.GetId(Id)); // Caso ele exista, retorna as informações do cliente
            }
            return NotFound("Cliente não cadastrado no banco de dados"); // Caso não exista, retorna NotFound
        }
        
        // Método pelo qual exclui um cliente pelo seu Id
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid Id)
        {
            Cliente ClienteProcurado = _clienteService.ProcuraCliente(Id); // Procura o cliente

            if (ClienteProcurado != null)
            {
                _clienteService.Delete(ClienteProcurado); // Caso ele exista, o cliente é deletado
                return NoContent(); // Ao final retorna NoContent
            }
            return NotFound("Cliente não cadastrado no banco de dados"); // Caso não exista, retorna NotFound
        }

        // Método para atualizar seu cliente pelo seu Id
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AtualizaId(Guid Id, [FromBody] ClienteDTO clienteDTO)
        {
            var result = _clienteService.VerificaErros(clienteDTO); // Faz a veirificação das informações enviadas
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros); // Caso tenha algum erro nas informações, retorna BadRequest
            }

            Cliente ClienteProcurado = _clienteService.ProcuraCliente(Id); // Procura o cliente a ser atualizado pelo seu Id
            ViaCepDTO viaCepDTO = _clienteService.BuscaCep(clienteDTO.Cep); // Procura o novo cep
            if (viaCepDTO.localidade != null)
            {
                if (ClienteProcurado != null)
                {
                    ClienteAtualizaDTO clienteAtualizaDTO = _clienteService.AtualizaCidade(clienteDTO, ClienteProcurado);

                    if (clienteAtualizaDTO == null) // Caso seja null, é porque a cidade não existe
                    {
                        CidadeDTO novaCidade = new CidadeDTO();
                        novaCidade.Nome = viaCepDTO.localidade;
                        novaCidade.Estado = viaCepDTO.uf;
                        _cidadeServices.CadastrarCidade(novaCidade); // Então primeiro é criado a cidade
                    }
                    clienteAtualizaDTO = _clienteService.AtualizaCidade(clienteDTO, ClienteProcurado); // Para depois o cliente ser atualizado

                    return NoContent(); // Retorna NoContent
                }
                return NotFound("Cliente não cadastrado no banco de dados"); // Retorna NotFound, caso tente atualizar um cliente inexistente
            }
            return NotFound("Não foi possível encontrar o cep desejado"); // Caso o cep não seja encontrado no ViaCep, retorna NotFound
        }
    }
}
