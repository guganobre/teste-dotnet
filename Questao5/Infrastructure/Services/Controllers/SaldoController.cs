using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Handlers;
using Questao5.Domain.Language;
using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoController : ControllerBase
    {
        private readonly IContaCorrenteHandler handler;

        public SaldoController(IContaCorrenteHandler handler)
        {
            this.handler = handler;
        }

        [HttpGet("{idContaCorrente}")]
        [SwaggerOperation(
            Summary = "Consultar saldo de uma conta corrente",
            Description = "Este endpoint consulta o saldo atual de uma conta corrente ativa."
        )]
        [ProducesResponseType(typeof(SaldoContaCorrenteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([SwaggerSchema("O identificador único da conta corrente.")] string idContaCorrente)
        {
            try
            {
                if (Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey))
                {
                    return Ok(await handler.GetSaldo(idempotencyKey, new SaldoContaCorrenteRequest(idContaCorrente)));
                }
                else
                {
                    return BadRequest("Idempotency-Key não foi informada ou não é um Guid valido.");
                }
            }
            catch (ArgumentException arg)
            {
                return BadRequest($"{MensagemErro.GetErroSaldo(arg.Message)}, código do erro: {arg.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}