using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repository
{
    public class MovimentoRepository : BaseRepository, IMovimentoRepository
    {
        private string insert = @"
                        INSERT INTO movimento (
                            idmovimento,
                            idcontacorrente,
                            datamovimento,
                            tipomovimento,
                            valor
                        )
                        VALUES (
                            @idmovimento,
                            @idcontacorrente,
                            @datamovimento,
                            @tipomovimento,
                            @valor
                        );";

        public MovimentoRepository(DatabaseConfig databaseConfig) : base(databaseConfig)
        {
        }

        public async Task<Movimento> Save(Movimento entity)
        {
            try
            {
                using (var con = new SqliteConnection(config.Name))
                {
                    var restul = await con.ExecuteAsync(insert,
                        new
                        {
                            idmovimento = entity.IdMovimento,
                            idcontacorrente = entity.IdContaCorrente,
                            datamovimento = entity.DataMovimento,
                            tipomovimento = entity.TipoMovimento,
                            valor = entity.Valor,
                        });

                    if (restul > 0)
                    {
                        return entity;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}