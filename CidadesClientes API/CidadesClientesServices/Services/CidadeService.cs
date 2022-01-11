using AutoMapper;
using CidadesClientesServices.Context;
using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CidadesClientesServices.Services
{
    public class CidadeService : ICidadeServices
    {
        private ClienteCidadeDbContext _context;
        private IMapper _mapper;

        public CidadeService(ClienteCidadeDbContext Contexto, IMapper mapper)
        {
            _context = Contexto;
            _mapper = mapper;
        }

        public void AtualizaCidade(CidadeDTO cidadeDTO, Cidade cidadeParaAtualizar)
        {
            _mapper.Map(cidadeDTO, cidadeParaAtualizar);
            _context.SaveChanges();
        }

        public CidadeRetornaDTO CadastrarCidade(CidadeDTO novaCidade)
        {
            CidadeRetornaDTO cidadeRetornaDTO = new CidadeRetornaDTO();
            Cidade cidade = _mapper.Map<Cidade>(novaCidade);
            _context.Cidades.Add(cidade);
            _context.SaveChanges();

            cidadeRetornaDTO.Id = cidade.Id;

            return cidadeRetornaDTO;
        }

        public void Delete(Cidade cidadeRemovida)
        {
            _context.Remove(cidadeRemovida);
            _context.SaveChanges();
        }

        public IEnumerable<CidadeDTO> GetAll()
        {
            IEnumerable<Cidade> TodasCidades = _context.Cidades;
            var ListaCidadesDTO = _mapper.Map<IEnumerable<CidadeDTO>>(TodasCidades);

            return ListaCidadesDTO;
        }

        public Cidade GetId(Guid Id)
        {
            Cidade cidadeProcurada = _context.Cidades.FirstOrDefault(C => C.Id == Id);

            return cidadeProcurada;
        }

        public CidadeDTO VerificaIgualdade(string nomeCidade, string estadoCidade)
        {
            var ContemCidade = _context.Cidades.FirstOrDefault(C => C.Nome == nomeCidade && C.Estado == estadoCidade);

            if (ContemCidade != null)
            {
                CidadeDTO cidadeRetorno = _mapper.Map<CidadeDTO>(ContemCidade);
                return cidadeRetorno;
            }
            return null;
        }
    }
}
