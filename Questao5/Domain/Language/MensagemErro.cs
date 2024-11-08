namespace Questao5.Domain.Language
{
    public static class MensagemErro
    {
        public const string INVALID_ACCOUNT = "INVALID_ACCOUNT";
        public const string INVALID_VALUE = "INVALID_VALUE";
        public const string INVALID_TYPE = "INVALID_TYPE";
        public const string INACTIVE_ACCOUNT = "INACTIVE_ACCOUNT";

        public static string GetErro(string erro)
        {
            switch (erro)
            {
                case INACTIVE_ACCOUNT:
                    return "Apenas contas correntes ativas podem receber movimentação";

                case INVALID_ACCOUNT:
                    return "Apenas contas correntes cadastradas podem receber movimentação";

                case INVALID_VALUE:
                    return "Apenas valores positivos podem ser recebidos";

                case INVALID_TYPE:
                    return "Apenas os tipos “débito” ou “crédito” podem ser aceitos";

                default:
                    return "";
            }
        }

        public static string GetErroSaldo(string erro)
        {
            switch (erro)
            {
                case INACTIVE_ACCOUNT:
                    return "Apenas contas correntes ativas podem consultar o saldo";

                case INVALID_ACCOUNT:
                    return "Apenas contas correntes cadastradas podem consultar o saldo";

                default:
                    return "";
            }
        }
    }
}