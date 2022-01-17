using AutoMapper;
using CidadesClientes_API.Profiles;
using CidadesClientesServices.Context;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.DTOS.ClienteDTOS;
using CidadesClientesServices.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestesCidadesClientesAPI
{
    public class CidadeTest
    {
        [Fact]
        public void QuandoInformacoesCompletasCidadeNaoNula()
        {
            // arrange
            CidadeDTO cidadeDTO = new CidadeDTO();
            cidadeDTO.Nome = "Nova Lima";
            cidadeDTO.Estado = "MG";

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
            var cidadeService = new CidadeService(memoryContext, mapper);

            // act
            CidadeRetornaDTO cidadeRetornaDTO = cidadeService.CadastrarCidade(cidadeDTO);

            // assert
            Assert.NotNull(cidadeRetornaDTO);
        }

        [Fact]
        public void QuandoInformacoesIncomplestasCidadeNaoValida()
        {
            // arrange
            CidadeDTO cidadeDTO = new CidadeDTO();
            cidadeDTO.Nome = "Nova Lima";

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
            var cidadeService = new CidadeService(memoryContext, mapper);

            // act
            var erros = cidadeService.VerificaErros(cidadeDTO);

            // assert
            Assert.False(erros.IsValid);
        }
    }
}
