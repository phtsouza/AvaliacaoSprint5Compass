using AutoMapper;
using CidadesClientes_API.Validators;
using CidadesClientesServices.Context;
using CidadesClientesServices.Contracts;
using CidadesClientesServices.DTOS;
using CidadesClientesServices.Models;
using FluentValidation.Results;
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

        // Procedimeno pelo qual atualiza uma cidade, com informações passadas por parâmetro
        public void AtualizaCidade(CidadeDTO cidadeDTO, Cidade cidadeParaAtualizar)
        {
            _mapper.Map(cidadeDTO, cidadeParaAtualizar);
            _context.SaveChanges();
        }

        // Procedimento pelo qual cadastra uma cidade no banco de dados
        public CidadeRetornaDTO CadastrarCidade(CidadeDTO novaCidade)
        {
            CidadeRetornaDTO cidadeRetornaDTO = new CidadeRetornaDTO();
            Cidade cidade = _mapper.Map<Cidade>(novaCidade);
            _context.Cidades.Add(cidade);
            _context.SaveChanges();

            cidadeRetornaDTO.Id = cidade.Id;

            return cidadeRetornaDTO;
        }

        // Procedimento pelo qual deleta uma cidade
        public void Delete(Cidade cidadeRemovida)
        {
            _context.Remove(cidadeRemovida);
            _context.SaveChanges();
        }

        // Procedimento pelo qual retorna uma lista com todas as cidades cadastradas no banco de dados
        public IEnumerable<CidadeDTO> GetAll()
        {
            IEnumerable<Cidade> TodasCidades = _context.Cidades;
            var ListaCidadesDTO = _mapper.Map<IEnumerable<CidadeDTO>>(TodasCidades);

            return ListaCidadesDTO;
        }

        // Procedimento pelo qual retorna a cidade pelo id passado por parâmetro
        public Cidade GetId(Guid Id)
        {
            Cidade cidadeProcurada = _context.Cidades.FirstOrDefault(C => C.Id == Id);

            return cidadeProcurada;
        }

        // Procedimento pelo qual faz a validação das informações passada para o cadastro ou atualização das cidades
        public ValidationResult VerificaErros(CidadeDTO cidadeDTO)
        {
            var cidadeValidator = new CidadeValidator();
            var result = cidadeValidator.Validate(cidadeDTO);

            return result;
        }

        // Procedimento criado para verificar se uma cidade já existe no banco de dados
        public CidadeDTO VerificaIgualdade(string nomeCidade, string estadoCidade)
        {
            var ContemCidade = _context.Cidades.FirstOrDefault(C => C.Nome == nomeCidade && C.Estado == estadoCidade);

            if (ContemCidade != null)
            {
                CidadeDTO cidadeRetorno = _mapper.Map<CidadeDTO>(ContemCidade);
                return cidadeRetorno; // Caso exista, retorna a cidade já existente
            }
            return null; // Caso não exista retorna null
        }
    }
}
