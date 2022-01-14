using AutoMapper;
using CidadesClientes_API.Profiles;
using CidadesClientesServices.Context;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestesCidadesClientesAPI
{
    public class ViaCepTest
    {
        [Fact]
        public void QuandoCepInvalidoErroRetornaTrue()
        {
            // arrange
            var cep = "12345678";

            var memoryDatabase = new DbContextOptionsBuilder<ClienteCidadeDbContext>()
                .UseInMemoryDatabase("test3")
                .Options;
            var memoryContext = new ClienteCidadeDbContext(memoryDatabase);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CidadeProfile>();
                cfg.AddProfile<ClienteProfile>();
            });

            var mapper = config.CreateMapper();

            var clienteService = new ClienteService(memoryContext, mapper);

            // act
            ViaCepDTO cepDTO = clienteService.BuscaCep(cep);

            // assert
            Assert.True(cepDTO.erro);
        }
    }
}
