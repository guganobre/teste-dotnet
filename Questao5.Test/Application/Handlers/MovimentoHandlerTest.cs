using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Handlers;
using Questao5.Domain.Language;
using Questao5.Domain.Sqlite;
using System.Drawing;

namespace Questao5.Test.Application.Handlers
{
    public class MovimentoHandlerTest
    {
        private readonly IMovimentoHandler handler;
        private readonly IContaCorrenteHandler contaHandler;

        public MovimentoHandlerTest(
            IMovimentoHandler handler,
            IContaCorrenteHandler contaHandler,
            IDatabaseBootstrap databaseBootstrap)
        {
            this.handler = handler;
            this.contaHandler = contaHandler;
            databaseBootstrap.Setup();
        }

        [Fact]
        public async void Movimento()
        {
            var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";

            var result = await handler.SaveAync(Guid.NewGuid().ToString(), new CreateMovimentoRequest(idConta, (char)TipoMovimento.Credito, 1));

            Assert.NotNull(result);
        }

        [Fact]
        public async void Movimento_TESTE_Idempotencia()
        {
            decimal valor = 1;

            var idempotenciaKey = Guid.NewGuid().ToString();
            var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";

            var saldo1 = await contaHandler.GetSaldo(Guid.NewGuid().ToString(), new SaldoContaCorrenteRequest(idContaCorrente: idConta));

            var result1 = await handler.SaveAync(idempotenciaKey, new CreateMovimentoRequest(idConta, (char)TipoMovimento.Credito, valor));

            var result2 = await handler.SaveAync(idempotenciaKey, new CreateMovimentoRequest(idConta, (char)TipoMovimento.Credito, valor));

            if (!result1.Equals(result1))
            {
                Assert.False(true, "Idempotência incorreta");
            }

            var saldo2 = await contaHandler.GetSaldo(Guid.NewGuid().ToString(), new SaldoContaCorrenteRequest(idContaCorrente: idConta));

            Assert.True(saldo1.Saldo + valor == saldo2.Saldo, "Idempotência incorreta, a transação foi processada duas vezes");
        }

        [Theory]
        [InlineData(200, TipoMovimento.Credito)]
        [InlineData(200, TipoMovimento.Debito)]
        public async void Movimento_SALDO(decimal valor, TipoMovimento tipoMovimento)
        {
            var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";

            var saldo1 = await contaHandler.GetSaldo(Guid.NewGuid().ToString(), new SaldoContaCorrenteRequest(idContaCorrente: idConta));

            var result = await handler.SaveAync(Guid.NewGuid().ToString(), new CreateMovimentoRequest(idConta, (char)tipoMovimento, valor));

            var saldo2 = await contaHandler.GetSaldo(Guid.NewGuid().ToString(), new SaldoContaCorrenteRequest(idContaCorrente: idConta));

            Assert.NotNull(result);

            if (tipoMovimento == TipoMovimento.Credito)
            {
                Assert.True(saldo1.Saldo + valor == saldo2.Saldo, "Não foi possível validar o crédito");
            }

            if (tipoMovimento == TipoMovimento.Debito)
            {
                Assert.True(saldo1.Saldo - valor == saldo2.Saldo, "Não foi possível validar o debito");
            }
        }

        [Theory]
        [InlineData("F475F943-7067-ED11-A06B-7E5DFA4A16C9")]
        public void Movimento_INACTIVE_ACCOUNT(string idConta)
        {
            var idempotenciaKey = Guid.NewGuid().ToString();

            var cmdCredito = new CreateMovimentoRequest(idConta, (char)TipoMovimento.Credito, 100);

            Assert.ThrowsAsync<ArgumentException>(() => handler.SaveAync(idempotenciaKey, cmdCredito)).ValidarMensagemAsync(MensagemErro.INACTIVE_ACCOUNT);
        }

        [Theory]
        [InlineData("066e6702-e3ec-48dc-8021-de9d8bf5ff3d")]
        [InlineData("")]
        [InlineData(null)]
        public void Movimento_INVALID_ACCOUNT(string idContaCorrente)
        {
            var idempotenciaKey = Guid.NewGuid().ToString();
            var command = new CreateMovimentoRequest(idContaCorrente, (char)TipoMovimento.Credito, valor: 20);

            Assert.ThrowsAsync<ArgumentException>(() => handler.SaveAync(idempotenciaKey, command)).ValidarMensagemAsync(MensagemErro.INVALID_ACCOUNT);
        }

        [Theory]
        [InlineData(0, TipoMovimento.Credito)]
        [InlineData(0, TipoMovimento.Debito)]
        [InlineData(-2, TipoMovimento.Credito)]
        [InlineData(-2, TipoMovimento.Debito)]
        public void Movimento_INVALID_VALUE(decimal valor, TipoMovimento tipoMovimento)
        {
            var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var idempotenciaKey = Guid.NewGuid().ToString();
            var cmdCredito = new CreateMovimentoRequest(idConta, (char)tipoMovimento, valor);

            Assert.ThrowsAsync<ArgumentException>(() => handler.SaveAync(idempotenciaKey, cmdCredito)).ValidarMensagemAsync(MensagemErro.INVALID_VALUE);
        }

        [Theory]
        [InlineData('E')]
        [InlineData(' ')]
        [InlineData(null)]
        public void Movimento_INVALID_TYPE(char tipoMovimento)
        {
            var idConta = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var idempotenciaKey = Guid.NewGuid().ToString();
            var cmdCredito = new CreateMovimentoRequest(idConta, tipoMovimento, 100);

            Assert.ThrowsAsync<ArgumentException>(() => handler.SaveAync(idempotenciaKey, cmdCredito)).ValidarMensagemAsync(MensagemErro.INVALID_TYPE);
        }
    }
}