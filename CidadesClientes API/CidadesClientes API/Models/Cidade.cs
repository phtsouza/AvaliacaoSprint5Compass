using System;
using System.Collections.Generic;

namespace CidadesClientes_API.Models
{
    public class Cidade
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public IList<Cliente> clientes { get; set; }
    }
}
