using NSubstitute.Core;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Handlers;
using Questao5.Domain.Language;
using Questao5.Domain.Sqlite;
using System.Threading;

namespace Questao5.Test.Application.Handlers
{
    public class ContaCorrenteHandlerTest
    {
        private readonly IContaCorrenteHandler handler;

        public ContaCorrenteHandlerTest(IContaCorrenteHandler handler, IDatabaseBootstrap databaseBootstrap)
        {
            this.handler = handler;

            databaseBootstrap.Setup();
        }

        [Fact]
        public void Saldo()
        {
            var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var idempotenciaKey = Guid.NewGuid().ToString();

            var query = new SaldoContaCorrenteRequest(idConta);

            var result = handler.GetSaldo(idempotenciaKey, query);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("F475F943-7067-ED11-A06B-7E5DFA4A16C9")]
        public void Saldo_INACTIVE_ACCOUNT(string idContaCorrente)
        {
            var idempotenciaKey = Guid.NewGuid().ToString();

            var query = new SaldoContaCorrenteRequest(idContaCorrente);

            Assert.ThrowsAsync<ArgumentException>(() => handler.GetSaldo(idempotenciaKey, query)).ValidarMensagemAsync(MensagemErro.INACTIVE_ACCOUNT);
        }

        [Theory]
        [InlineData("066e6702-e3ec-48dc-8021-de9d8bf5ff3d")]
        [InlineData("")]
        [InlineData(null)]
        public void Saldo_INVALID_ACCOUNT(string idContaCorrente)
        {
            var idempotenciaKey = Guid.NewGuid().ToString();
            var query = new SaldoContaCorrenteRequest(idContaCorrente);

            Assert.ThrowsAsync<ArgumentException>(() => handler.GetSaldo(idempotenciaKey, query)).ValidarMensagemAsync(MensagemErro.INVALID_ACCOUNT);
        }

        [Fact]
        public async void Saldo_TESTE_Idempotencia()
        {
            var idempotenciaKey = Guid.NewGuid().ToString();
            var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";

            var query1 = await handler.GetSaldo(idempotenciaKey, new SaldoContaCorrenteRequest(idConta));

            var query2 = await handler.GetSaldo(idempotenciaKey, new SaldoContaCorrenteRequest(idConta));

            if (!query1.Equals(query2))
            {
                Assert.False(true, "Idempotência incorreta");
            }
        }
    }
}