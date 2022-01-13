using AutoMapper;
using CidadesClientes_API.Profiles;
using CidadesClientesServices.Context;
using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.DTOS.ClienteDTOS;
using CidadesClientesServices.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace TestesCidadesClientesAPI
{
    public class ClienteTeste
    {
        [Fact]
        public void QuandoClienteTemCidadeCadastradaClienteNaoNulo()
        {
            // arrange
            CidadeDTO cidadeDTO = new CidadeDTO();
            cidadeDTO.Estado = "MG";
            cidadeDTO.Nome = "Nova Lima";

            var cliente = new ClienteDTO();
            var nascimento = new DateTime(2003, 05, 02);
            var cep = "34000159";
            cliente.Nome = "Pedro";
            cliente.Nascimento = nascimento;

            var memoryDatabase = new DbContextOptionsBuilder<ClienteCidadeDbContext>()
                .UseInMemoryDatabase("test1")
                .Options;
            var memoryContext = new ClienteCidadeDbContext(memoryDatabase);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CidadeProfile>();
                cfg.AddProfile<ClienteProfile>();
            });

            var mapper = config.CreateMapper();

            var clienteService = new ClienteService(memoryContext, mapper);
            var cidadeService = new CidadeService(memoryContext, mapper);

            ViaCepDTO cepDTO = clienteService.BuscaCep(cep);
            // act
            cidadeService.CadastrarCidade(cidadeDTO);
            ClienteRetornaDTO clienteRetornaDTO = clienteService.Cadastra(cliente, cepDTO);

            // assert
            var customers = clienteService.GetAll();

            Assert.NotNull(clienteRetornaDTO);
        }

        [Fact]
        public void QuandoCadastrarComCepInvalidoClienteNulo()
        {
            // arrange
            var cliente = new ClienteDTO();
            var nascimento = new DateTime(2003, 05, 02);
            var cep = "12345678";
            cliente.Nome = "Pedro";
            cliente.Nascimento = nascimento;

            var memoryDatabase = new DbContextOptionsBuilder<ClienteCidadeDbContext>()
                .UseInMemoryDatabase("test2")
                .Options;
            var memoryContext = new ClienteCidadeDbContext(memoryDatabase);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CidadeProfile>();
                cfg.AddProfile<ClienteProfile>();
            });

            var mapper = config.CreateMapper();

            var clienteService = new ClienteService(memoryContext, mapper);

            ViaCepDTO cepDTO = clienteService.BuscaCep(cep);

            // act
            ClienteRetornaDTO clienteRetornaDTO = clienteService.Cadastra(cliente, cepDTO);

            // assert
            Assert.Null(clienteRetornaDTO);
        }
    }
}
