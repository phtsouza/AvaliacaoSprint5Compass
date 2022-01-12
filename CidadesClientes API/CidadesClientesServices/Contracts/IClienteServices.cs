using CidadesClientesServices.DTOS;
using CidadesClientesServices.DTOS.ClienteDTOS;
using CidadesClientesServices.Models;
using System;
using System.Collections.Generic;

namespace CidadesClientesServices.Contracts
{
    public interface IClienteServices
    {
        public ViaCepDTO BuscaCep(string CEP);
        ClienteRetornaDTO Cadastra(ClienteDTO clienteDTO, ViaCepDTO viaCepDTO);
        IEnumerable<ClienteDTO> GetAll();
        Cliente ProcuraCliente(Guid Id);
        ClienteDTO GetId(Guid Id);
        void Delete(Cliente clienteProcurado);
        void AtualizaCidade(ClienteDTO clienteDTO, Cliente clienteProcurado);
    }
}
