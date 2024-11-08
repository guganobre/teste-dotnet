using Microsoft.Data.Sqlite;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Sqlite;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace Questao5.Infrastructure.Repository
{
    public class CommandStroreRepository : BaseRepository, ICommandRepository
    {
        public CommandStroreRepository(DatabaseConfig databaseConfig) : base(databaseConfig)
        {
        }

        public TResult? GetByKey<TResult>(string? chave)
        {
            var query = @"
                    SELECT resultado
                    FROM idempotencia
                    where chave_idempotencia = @chave_idempotencia";

            using (var con = new SqliteConnection(config.Name))
            {
                var result = con.QueryFirstOrDefault<string>(query, new { chave_idempotencia = chave });

                if (result == null)
                {
                    return default;
                }
                else
                {
                    return JsonSerializer.Deserialize<TResult>(result);
                }
            }
        }

        public async Task<bool> SaveAsync(CreateCommandStoreRequest command)
        {
            try
            {
                var insert = @"
                        INSERT INTO idempotencia (
                             chave_idempotencia,
                             requisicao,
                             resultado
                         )
                         VALUES (
                             @chave_idempotencia,
                             @requisicao,
                             @resultado
                         );";

                using (var con = new SqliteConnection(config.Name))
                {
                    var restul = await con.ExecuteAsync(insert,
                    new
                    {
                        chave_idempotencia = command.Chave,
                        requisicao = JsonSerializer.Serialize(command.Requisicao),
                        resultado = JsonSerializer.Serialize(command.Resultado),
                    });

                    if (restul > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}