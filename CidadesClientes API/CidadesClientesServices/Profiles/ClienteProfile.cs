using AutoMapper;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.Models;

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
