using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;
using System;

namespace Questao5.Infrastructure.Repository
{
    public class ContaCorrenteRepository : BaseRepository, IContaCorrenteRepository
    {
        public ContaCorrenteRepository(DatabaseConfig databaseConfig) : base(databaseConfig)
        {
        }

        public bool Exists(string idContaCorrente)
        {
            var query = @"SELECT 1 FROM contacorrente WHERE idcontacorrente = @idcontacorrente";

            using (var con = new SqliteConnection(config.Name))
            {
                var result = con.QueryFirstOrDefault<string>(query, new { idcontacorrente = idContaCorrente });

                return (result == null) ? false : true;
            }
        }

        public TResult GetSaldo<TResult>(string idContaCorrente)
        {
            var query = @"
SELECT
       c.numero,
       c.nome as titular,
       IFNULL(SUM(CASE WHEN m.tipomovimento = 'D' THEN m.valor * - 1 ELSE m.valor END), 0) AS saldo
FROM contacorrente AS c
LEFT JOIN movimento AS m ON c.idcontacorrente = m.idcontacorrente
WHERE c.idcontacorrente = @idcontacorrente
GROUP BY c.numero, c.nome;
";

            using (var con = new SqliteConnection(config.Name))
            {
                var result = con.QueryFirstOrDefault<TResult>(query, new { idcontacorrente = idContaCorrente });

                return result;
            }
        }

        public bool IsActive(string idContaCorrente)
        {
            var query = @"SELECT 1 FROM contacorrente WHERE idcontacorrente = @idcontacorrente and ativo = @ativo";

            using (var con = new SqliteConnection(config.Name))
            {
                var result = con.QueryFirstOrDefault<string>(query, new { idcontacorrente = idContaCorrente, ativo = true });

                return (result == null) ? false : true;
            }
        }
    }
}