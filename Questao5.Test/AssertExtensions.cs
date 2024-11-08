namespace Questao5.Test
{
    internal static class AssertExtensions
    {
        public static void ValidarMensagem(this ArgumentException ex, string mesagem)
        {
            if (ex.Message.Equals(mesagem))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, $"A mensagem esperada é {mesagem}");
            }
        }

        public static async void ValidarMensagemAsync(this Task<ArgumentException> ex, string mesagem)
        {
            if ((await ex).Message.Equals(mesagem))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true, $"A mensagem esperada é {mesagem}");
            }
        }
    }
}