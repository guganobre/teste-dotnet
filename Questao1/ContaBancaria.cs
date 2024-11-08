using System;
using System.Globalization;

namespace Questao1
{
    internal class ContaBancaria
    {
        private double _taxaSaque = 3.5;

        /// <summary>
        /// Numero único e identificador da conta bancária.
        /// </summary>
        public int Numero { get; private set; }

        /// <summary>
        /// Nome do titular.
        /// </summary>
        public string Titular { get; set; }

        /// <summary>
        /// Saldo em conta.
        /// </summary>
        public double Saldo { get; private set; }

        /// <summary>
        /// Método para criação
        /// </summary>
        /// <param name="numero">Numero inicial para criação da conta bancária</param>
        /// <param name="titular">Nome do titular</param>
        /// <param name="depositoInicial">Valor inicial para criação da conta</param>
        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            Numero = numero;
            Titular = titular;

            if (depositoInicial < 0)
            {
                throw new Exception("Não é possível criar uma conta com o saldo negativo");
            }
            else
            {
                Saldo = depositoInicial;
            }
        }

        /// <summary>
        /// Realiza depósito em conta.
        /// </summary>
        /// <param name="quantia">Valor a ser depoitado</param>
        internal void Deposito(double quantia)
        {
            this.Saldo += quantia;
        }

        /// <summary>
        /// Realiza saque em conta. Obs.: Ao realizar o saque será cobrado uma taxa parametrizada como tarifa para o saque.
        /// </summary>
        /// <param name="quantia">Valor a ser sacado.</param>
        internal void Saque(double quantia)
        {
            Saldo -= (quantia + _taxaSaque);
        }

        /// <summary>
        /// Altera o nome do titular da conta.
        /// </summary>
        /// <param name="novoNome">Nome que substituirá o antigo</param>
        public void AlterarNomeTitular(string novoNome)
        {
            Titular = novoNome;
        }

        /// <summary>
        /// converte os dados da conta bancária em string
        /// </summary>
        /// <returns>dados da conta.</returns>
        public override string ToString()
        {
            return $"Conte {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}