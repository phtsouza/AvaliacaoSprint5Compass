using AutoMapper;
using CidadesClientesServices.Context;
using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.DTOS.ClienteDTOS;
using CidadesClientesServices.Models;
using CidadesClientesServices.Validators;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace CidadesClientesServices.Services
{
    public class ClienteService : IClienteServices
    {
        private ClienteCidadeDbContext _context;
        private IMapper _mapper;

        public ClienteService(ClienteCidadeDbContext Contexto, IMapper mapper)
        {
            _context = Contexto;
            _mapper = mapper;
        }

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

        public ClienteRetornaDTO Cadastra(ClienteDTO clienteDTO, ViaCepDTO viaCepDTO)
        {
            ClienteRetornaDTO clienteRetornaDTO = new ClienteRetornaDTO();
            Cliente cliente = _mapper.Map<Cliente>(clienteDTO);

            cliente.Logradouro = viaCepDTO.logradouro;
            cliente.Bairro = viaCepDTO.bairro;

            var ContemCidade = _context.Cidades.FirstOrDefault(C => C.Nome == viaCepDTO.localidade && C.Estado == viaCepDTO.uf);
            if (ContemCidade != null)
            {
                cliente.CidadeId = ContemCidade.Id;
                
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                clienteRetornaDTO.Id = cliente.Id;

                return clienteRetornaDTO;
            }
            return null;
        }

        public IEnumerable<ClienteDTO> GetAll()
        {
            IEnumerable<Cliente> Clientes = _context.Clientes.Include(C => C.cidade);
            var ClientesDTO = _mapper.Map<IEnumerable<ClienteDTO>>(Clientes);

            return ClientesDTO;
        }

        public Cliente ProcuraCliente(Guid Id)
        {
            Cliente ClienteProcurado = _context.Clientes.Include(C => C.cidade).FirstOrDefault(Cl => Cl.Id == Id);

            return ClienteProcurado;
        }

        public ClienteDTO GetId(Guid Id)
        {
            Cliente ClienteProcurado = ProcuraCliente(Id);

            ClienteDTO ClienteRetornado = _mapper.Map<ClienteDTO>(ClienteProcurado);

            return ClienteRetornado;
        }

        public void Delete(Cliente clienteProcurado)
        {
            _context.Remove(clienteProcurado);
            _context.SaveChanges();
        }

        public void AtualizaCidade(ClienteDTO clienteDTO, Cliente clienteProcurado)
        {
            var cepOriginal = clienteProcurado.Cep;

            _mapper.Map(clienteDTO, clienteProcurado);

            if (cepOriginal != clienteDTO.Cep)
            {
                ViaCepDTO viaCepDTO = BuscaCep(clienteDTO.Cep);

                clienteProcurado.Logradouro = viaCepDTO.logradouro;

                clienteProcurado.Bairro = viaCepDTO.bairro;
            }
            _context.SaveChanges();
        }

        public ValidationResult VerificaErros(ClienteDTO clienteDTO)
        {
            var cidadeValidator = new ClienteValidator();
            var result = cidadeValidator.Validate(clienteDTO);

            return result;
        }
    }
}
