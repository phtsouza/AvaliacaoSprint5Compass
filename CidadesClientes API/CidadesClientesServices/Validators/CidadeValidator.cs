using CidadesClientesServices.DTOS;
using FluentValidation;

namespace CidadesClientes_API.Validators
{
    public class CidadeValidator : AbstractValidator<CidadeDTO>
    {
        public CidadeValidator()
        {
            RuleFor(c => c.Nome).NotEmpty().WithMessage("Preencha o nome da cidade");
            RuleFor(c => c.Estado).NotEmpty().WithMessage("Preencha o nome do estado");
            RuleFor(c => c.Estado).Length(2).WithMessage("Preencha apenas a sigla do estado");
        }
    }
}
