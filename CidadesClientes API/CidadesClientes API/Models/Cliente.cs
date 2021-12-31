using System;

namespace CidadesClientes_API.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        public Cidade cidade { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
    }
}

