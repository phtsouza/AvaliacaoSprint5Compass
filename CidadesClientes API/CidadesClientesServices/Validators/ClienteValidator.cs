using CidadesClientesServices.DTOS;
using FluentValidation;

namespace CidadesClientesServices.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteDTO>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.Nome).NotEmpty().WithMessage("Preencha o nome do cliente");
            RuleFor(c => c.Nascimento).NotEmpty().WithMessage("Preencha a data de nascimento");
            RuleFor(c => c.Cep).NotEmpty().Matches(@"^[0-9]{8}$").WithMessage("Cep em formato inválido. Exemplo formato correto: 12345678");
        }
    }
}