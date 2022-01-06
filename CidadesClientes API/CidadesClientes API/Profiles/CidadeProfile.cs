using AutoMapper;
using CidadesClientes_API.DTOS;
using CidadesClientes_API.Models;

namespace CidadesClientes_API.Profiles
{
    public class CidadeProfile : Profile
    {
        public CidadeProfile()
        {
            CreateMap<CidadeDTO, Cidade>();
            CreateMap<Cidade, CidadeDTO>();
        }
    }
}