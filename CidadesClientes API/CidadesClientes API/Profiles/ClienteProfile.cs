using AutoMapper;
using CidadesClientes_API.DTOS;
using CidadesClientes_API.Models;

namespace CidadesClientes_API.Profiles
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<ClienteDTO, Cliente>();
            CreateMap<Cliente, ClienteDTO>();
        }
    }
}
