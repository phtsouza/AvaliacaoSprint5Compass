using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.DTOS.ClienteDTOS;
using CidadesClientesServices.Models;
using FluentValidation.Results;
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

        [HttpPost]
        public IActionResult Post([FromBody] ClienteDTO clienteDTO)
        {
            var result = _clienteService.VerificaErros(clienteDTO);
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros);
            }

            ViaCepDTO viaCepDTO = _clienteService.BuscaCep(clienteDTO.Cep);

            if (viaCepDTO.localidade != null)
            {
                ClienteRetornaDTO clienteNovo = _clienteService.Cadastra(clienteDTO, viaCepDTO);

                if(clienteNovo == null)
                {
                    CidadeDTO novaCidade = new CidadeDTO();
                    novaCidade.Nome = viaCepDTO.localidade;
                    novaCidade.Estado = viaCepDTO.uf;
                    _cidadeServices.CadastrarCidade(novaCidade);
                }

                clienteNovo = _clienteService.Cadastra(clienteDTO, viaCepDTO);
                return CreatedAtAction(nameof(GetId), new { Id = clienteNovo.Id }, clienteNovo);
            }
            return NotFound("Não foi possível encontrar o cep desejado");
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_clienteService.GetAll());
        }
        
        [HttpGet("{Id}")]
        public IActionResult GetId(Guid Id)
        {
            
            Cliente ClienteProcurado = _clienteService.ProcuraCliente(Id);

            if(ClienteProcurado != null)
            {
                return Ok(_clienteService.GetId(Id));
            }
            return NotFound("Cliente não cadastrado no banco de dados");
        }
        
        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            Cliente ClienteProcurado = _clienteService.ProcuraCliente(Id);

            if (ClienteProcurado != null)
            {
                _clienteService.Delete(ClienteProcurado);
                return NoContent();
            }
            return NotFound("Cliente não cadastrado no banco de dados");
        }

        [HttpPut("{Id}")]
        public IActionResult AtualizaId(Guid Id, [FromBody] ClienteDTO clienteDTO)
        {
            var result = _clienteService.VerificaErros(clienteDTO);
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros);
            }

            Cliente ClienteProcurado = _clienteService.ProcuraCliente(Id);

            if (ClienteProcurado != null)
            {
                _clienteService.AtualizaCidade(clienteDTO, ClienteProcurado);
                
                return NoContent();
            }
            return NotFound("Cliente não cadastrado no banco de dados");
        }
    }
}
