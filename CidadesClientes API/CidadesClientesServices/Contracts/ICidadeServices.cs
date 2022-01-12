using CidadesClientesServices.DTOS;
using CidadesClientesServices.Models;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace CidadesClientesServices.Contracts
{
    public interface ICidadeServices
    {
        public CidadeDTO VerificaIgualdade(string nomeCidade, string estadoCidade);
        public CidadeRetornaDTO CadastrarCidade(CidadeDTO novaCidade);
        public IEnumerable<CidadeDTO> GetAll();
        public Cidade GetId(Guid id);
        public void Delete(Cidade cidadeRemovida);
        void AtualizaCidade(CidadeDTO cidadeDTO, Cidade cidadeParaAtualizar);
        ValidationResult VerificaErros(CidadeDTO cidadeDTO);
    }
}
