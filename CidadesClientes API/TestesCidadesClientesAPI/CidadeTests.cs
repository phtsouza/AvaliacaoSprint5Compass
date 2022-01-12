using CidadesClientesServices.Contracts;
using CidadesClientesServices.Models;
using System;
using Xunit;

namespace TestesCidadesClientesAPI
{
    public class CidadeTests
    {
        private IClienteServices _testCliente;

        public CidadeTests(IClienteServices clienteServices)
        {
            _testCliente = clienteServices;
        }

        [Fact]
        public void QuandoCadastrarNaoDeveRetornarNulo()
        {
            //arrange
            string cep = "34000159";
            var buscaCep = _testCliente.BuscaCep(cep);
            var cliente = new Cliente();
            cliente.Nome = "Teste";
            //execute
            cliente.Logradouro = buscaCep.logradouro;
            cliente.Bairro = buscaCep.bairro;
            //assert
            Assert.NotNull(cliente.Nome);
            Assert.NotNull(cliente.Logradouro);
            Assert.NotNull(cliente.Bairro);

        }
    }
}
