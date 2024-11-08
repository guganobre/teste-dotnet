namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class CreateCommandStoreRequest
    {
        public string? Chave { get; set; }
        public object? Requisicao { get; set; }
        public object? Resultado { get; set; }
    }
}