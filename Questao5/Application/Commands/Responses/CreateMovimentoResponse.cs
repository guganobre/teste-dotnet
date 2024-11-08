using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Application.Commands.Responses
{
    public class CreateMovimentoResponse
    {
        [SwaggerSchema("Id da movimentação gerada.")]
        public string? IdMovimento { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is CreateMovimentoResponse newObj)
            {
                return IdMovimento == newObj.IdMovimento;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdMovimento);
        }
    }
}