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

        // Procedimento pelo qual faz a busca de informações do cep informado por meio da API ViaCep
        public ViaCepDTO BuscaCep(string CEP)
        {
            string Url = $"http://www.viacep.com.br/ws/{CEP}/json/"; // Url da API
            WebRequest Requisicao = WebRequest.Create(Url);
            Requisicao.Method = "GET"; // Informa o tipo da reuisição

            using WebResponse resp = Requisicao.GetResponse();
            using var stream = resp.GetResponseStream();
            using StreamReader leitor = new StreamReader(stream); 

            string JsonViaCep = leitor.ReadToEnd(); // Faz a leitura dos dados retornados pela API
            string resultado = JsonViaCep;
            ViaCepDTO ViaCepData = JsonConvert.DeserializeObject<ViaCepDTO>(resultado); // Transforma os dados em um objeto ViaCEP

            return ViaCepData; // Retorna o objeto
        }

        // Procedimento pelo qual faz o cadastro de um cliente no banco de dados
        public ClienteRetornaDTO Cadastra(ClienteDTO clienteDTO, ViaCepDTO viaCepDTO)
        {
            ClienteRetornaDTO clienteRetornaDTO = new ClienteRetornaDTO();
            Cliente cliente = _mapper.Map<Cliente>(clienteDTO);

            cliente.Logradouro = viaCepDTO.logradouro;
            cliente.Bairro = viaCepDTO.bairro;

            var ContemCidade = _context.Cidades.FirstOrDefault(C => C.Nome == viaCepDTO.localidade && C.Estado == viaCepDTO.uf); // Verifica se a cidade existe
            if (ContemCidade != null)
            {
                cliente.CidadeId = ContemCidade.Id;
                
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                clienteRetornaDTO.Id = cliente.Id;

                return clienteRetornaDTO; // Caso exista, o cliente é adicionado e retorna o seu Id
            }
            return null; // Caso não exista, retorna null para que a cidade seja criada
        }

        // Procedimento pelo qual retorna todos os clientes já cadastrados
        public IEnumerable<ClienteDTO> GetAll()
        {
            IEnumerable<Cliente> Clientes = _context.Clientes.Include(C => C.cidade);
            var ClientesDTO = _mapper.Map<IEnumerable<ClienteDTO>>(Clientes);

            return ClientesDTO;
        }

        // Procedimento pelo qual retorna o cliente pelo seu Id
        public Cliente ProcuraCliente(Guid Id)
        {
            Cliente ClienteProcurado = _context.Clientes.Include(C => C.cidade).FirstOrDefault(Cl => Cl.Id == Id);

            return ClienteProcurado;
        }

        // Procedimento pelo ual retorna um clienteDTO pelo Id passado por parâmetro
        public ClienteDTO GetId(Guid Id)
        {
            Cliente ClienteProcurado = ProcuraCliente(Id);

            ClienteDTO ClienteRetornado = _mapper.Map<ClienteDTO>(ClienteProcurado);

            return ClienteRetornado;
        }

        // Procedimento pelo qual exclui um cliente
        public void Delete(Cliente clienteProcurado)
        {
            _context.Remove(clienteProcurado);
            _context.SaveChanges();
        }

        // Procedimento pelo qual atualiza um cliente
        public ClienteAtualizaDTO AtualizaCidade(ClienteDTO clienteDTO, Cliente clienteProcurado)
        {
            ClienteAtualizaDTO clienteAtualizaDTO = new ClienteAtualizaDTO();
            var cepOriginal = clienteProcurado.Cep;

            _mapper.Map(clienteDTO, clienteProcurado);

            if (cepOriginal != clienteDTO.Cep)
            {
                ViaCepDTO viaCepDTO = BuscaCep(clienteDTO.Cep); // Busca o novo cep, na API ViaCep

                var ContemCidade = _context.Cidades.FirstOrDefault(C => C.Nome == viaCepDTO.localidade && C.Estado == viaCepDTO.uf);
                if (ContemCidade != null)
                {
                    clienteProcurado.CidadeId = ContemCidade.Id;
                    clienteProcurado.Logradouro = viaCepDTO.logradouro;
                    clienteProcurado.Bairro = viaCepDTO.bairro;
                }
                else
                {
                    return null; // Caso a cidade não exista, retorna null para que ela seja criada
                }  
            }
            _context.SaveChanges();

            return clienteAtualizaDTO;
        }

        // Procedimento pelo qual faz a validação das informações passada para o cadastro ou atualização dos clientes 
        public ValidationResult VerificaErros(ClienteDTO clienteDTO)
        {
            var cidadeValidator = new ClienteValidator();
            var result = cidadeValidator.Validate(clienteDTO);

            return result;
        }
    }
}
