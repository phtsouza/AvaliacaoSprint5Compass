using AutoMapper;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.Models;

namespace CidadesClientes_API.Profiles
{
    public class CidadeProfile : Profile
    {
        public CidadeProfile()
        {
            CreateMap<CidadeDTO, Cidade>();
            CreateMap<Cidade, CidadeDTO>();
            CreateMap<Cidade, CidadeRetornaDTO>();
        }
    }
}