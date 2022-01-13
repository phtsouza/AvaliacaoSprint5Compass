using System;

namespace CidadesClientesServices.DTOS.ClienteDTOS
{
    public class ClienteAtualizaDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        public string Cep { get; set; }
    }
}
