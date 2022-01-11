using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CidadesClientesServices.Models
{
    public class Cliente
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        [ForeignKey("CidadeId")]
        public Cidade cidade { get; set; }
        public Guid CidadeId { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
    }
}

