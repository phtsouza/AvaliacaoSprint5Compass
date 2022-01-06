using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CidadesClientes_API.Models
{
    public class Cidade
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public ICollection<Cliente> clientes { get; set; } = new List<Cliente>();
    }
}
