using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Handlers;
using Questao5.Domain.Language;
using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentoController : ControllerBase
    {
        private readonly IMovimentoHandler handler;

        public MovimentoController(IMovimentoHandler handler)
        {
            this.handler = handler;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria um novo movimento",
            Description = "Este endpoint cria um movimento e retorna o resultado com o id da operação."
        )]
        [ProducesResponseType(typeof(CreateMovimentoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateMovimentoRequest request)
        {
            try
            {
                if (Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey))
                {
                    return Ok(await handler.SaveAync(idempotencyKey, request));
                }
                else
                {
                    return BadRequest("Idempotency-Key não foi informada ou não é um Guid valido.");
                }
            }
            catch (ArgumentException arg)
            {
                return BadRequest($"{MensagemErro.GetErro(arg.Message)}, código do erro: {arg.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}