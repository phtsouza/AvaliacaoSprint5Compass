using Microsoft.AspNetCore.Mvc;
using System;
using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.Models;
using CidadesClientes_API.Validators;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FluentValidation.Results;

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

        [HttpPost]
        public IActionResult Post([FromBody] CidadeDTO cidadeDTO)
        {
            var result = _cidadeServices.VerificaErros(cidadeDTO);
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros);
            }

            CidadeDTO NovaCidade = _cidadeServices.VerificaIgualdade(cidadeDTO.Nome, cidadeDTO.Estado);
            
            if(NovaCidade == null)
            {
                CidadeRetornaDTO novaCidadeDTO =_cidadeServices.CadastrarCidade(cidadeDTO);

                return CreatedAtAction(nameof(GetId), new { Id = novaCidadeDTO.Id }, novaCidadeDTO);
            }
            return BadRequest(new { Mensagem = "Cidade já existente no banco de dados!", Erro = true });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_cidadeServices.GetAll());
        }

        [HttpGet("{Id}")]
        public IActionResult GetId(Guid Id)
        {
            Cidade cidadeProcurada = _cidadeServices.GetId(Id);

            if (cidadeProcurada == null)
            {
                return NotFound("Cidade não cadastrada!");
            }
            else
            {
                return Ok(cidadeProcurada);
            }
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteId(Guid Id)
        {
            Cidade cidadeRemovida = _cidadeServices.GetId(Id);

            if(cidadeRemovida == null)
            {
                return NotFound("Cidade não cadastrada!");
            }

            try
            {
                _cidadeServices.Delete(cidadeRemovida);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest("Erro ao excluir a cidade!");
            }
            
        }
        
        [HttpPut("{Id}")]
        public IActionResult AtualizaId(Guid Id, [FromBody] CidadeDTO cidadeDTO)
        {
            var result = _cidadeServices.VerificaErros(cidadeDTO);
            IList<ValidationFailure> erros = result.Errors;

            if (!result.IsValid)
            {
                return BadRequest(erros);
            }

            Cidade cidadeParaAtualizar = _cidadeServices.GetId(Id);
            if (cidadeParaAtualizar == null)
            {
                return NotFound("Cidade não cadastrada!");
            }

            CidadeDTO novaCidade = _cidadeServices.VerificaIgualdade(cidadeDTO.Nome, cidadeDTO.Estado);

            if (novaCidade != null)
            {
                return BadRequest("Cidade já cadastrada!");
            }

            _cidadeServices.AtualizaCidade(cidadeDTO, cidadeParaAtualizar);
            
            return Ok("Cidade Atualizada!");
        }
        
    }
}
