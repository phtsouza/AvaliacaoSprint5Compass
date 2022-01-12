using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.DTOS.ClienteDTOS;
using CidadesClientesServices.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CidadesClientes_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private IClienteServices _clienteService;

        public ClienteController(IClienteServices clienteServices)
        {
            _clienteService = clienteServices;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClienteDTO clienteDTO)
        {
            ViaCepDTO viaCepDTO = _clienteService.BuscaCep(clienteDTO.Cep);

            if (viaCepDTO != null)
            {
                ClienteRetornaDTO clienteNovo = _clienteService.Cadastra(clienteDTO, viaCepDTO);

                if(clienteNovo != null)
                {
                    return CreatedAtAction(nameof(GetId), new { Id = clienteNovo.Id }, clienteNovo);
                }

                return BadRequest("Cidade inexistente no banco, primeiro adicione uma cidade para depois adicionar um cliente");
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
