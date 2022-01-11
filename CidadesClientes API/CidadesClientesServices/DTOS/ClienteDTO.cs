using System;

namespace CidadesClientesServices.DTOS
{
	public class ClienteDTO
	{
        public string Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        public string Cep { get; set; }
    }
}
