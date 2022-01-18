using Microsoft.AspNetCore.Mvc;
using System;
using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.Models;
using CidadesClientes_API.Validators;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace CidadesClientes_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private ICidadeServices _cidadeServices;

        public CidadeController(ICidadeServices cidadeServices)
        {
            _cidadeServices = cidadeServices;
        }

        // Método pelo qual adiciona uma cidade na base de dados
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] CidadeDTO cidadeDTO)
        {
            var result = _cidadeServices.VerificaErros(cidadeDTO); // Faz a verificação das informações enviadas
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros); // Caso tenha algum erro, retorna BadRequest
            }

            CidadeDTO NovaCidade = _cidadeServices.VerificaIgualdade(cidadeDTO.Nome, cidadeDTO.Estado); // Verifica se é uma cidade já existente no banco de dados
            
            if(NovaCidade == null)
            {
                CidadeRetornaDTO novaCidadeDTO =_cidadeServices.CadastrarCidade(cidadeDTO); // Caso cidade não exista, é criada no banco de dados

                return CreatedAtAction(nameof(GetId), new { Id = novaCidadeDTO.Id }, novaCidadeDTO); // Retorna o Id da nova cidade
            }
            return BadRequest(new { Mensagem = "Cidade já existente no banco de dados!", Erro = true });
        }

        // Método pelo qual retorna todas as cidades já cadastradas
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_cidadeServices.GetAll());
        }

        // Método pelo qual retorna a cidade pelo seu id
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetId(Guid Id)
        {
            Cidade cidadeProcurada = _cidadeServices.GetId(Id); // Procura a cidade pelo seu Id.

            if (cidadeProcurada == null)
            {
                return NotFound("Cidade não cadastrada!"); // Caso ela não exista, retorna NotFound
            }
            else
            {
                return Ok(cidadeProcurada); // Caso exista retorna informações da cidade
            }
        }

        // Método pelo qual Deleta uma cidade pelo seu Id
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteId(Guid Id)
        {
            Cidade cidadeRemovida = _cidadeServices.GetId(Id); // Procura a cidade pelo seu Id.

            if(cidadeRemovida == null)
            {
                return NotFound("Cidade não cadastrada!"); // Caso ela não exista, retorna NotFound
            }

            // Caso exista, ele tenta excluir a cidade
            try
            {
                _cidadeServices.Delete(cidadeRemovida);
                return NoContent(); // Se não ocorrer nenhum erro a cidade é excluida e retorna NoContent
            }
            catch(Exception ex)
            {
                return BadRequest("Erro ao excluir a cidade!"); // Caso ocorrar algum erro, retorna BadRequest
            }
            
        }

        // Método para atualizar uma cidade passando seu Id
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AtualizaId(Guid Id, [FromBody] CidadeDTO cidadeDTO)
        {
            var result = _cidadeServices.VerificaErros(cidadeDTO); // Faz a verificação das informações enviadas
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros); // Caso tenha algum erro, retorna BadRequest
            }

            Cidade cidadeParaAtualizar = _cidadeServices.GetId(Id); // Procura a cidade pelo seu Id
            if (cidadeParaAtualizar == null)
            {
                return NotFound("Cidade não cadastrada!"); // Caso a cidade não exista, retorna NotFound
            }

            CidadeDTO novaCidade = _cidadeServices.VerificaIgualdade(cidadeDTO.Nome, cidadeDTO.Estado); // Verifica se a cidade nova já existe no banco de dados

            if (novaCidade != null)
            {
                return BadRequest("Cidade já cadastrada!"); // Caso já exista retorna um BadRequest
            }

            _cidadeServices.AtualizaCidade(cidadeDTO, cidadeParaAtualizar); // Caso não exista a cidade é atualizada
            
            return Ok("Cidade Atualizada!"); // Retona um Ok, para quando funcionou corretamente
        }
        
    }
}
