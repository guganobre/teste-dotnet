using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repository
{
    public abstract class BaseRepository
    {
        protected DatabaseConfig config { get; }

        public BaseRepository(DatabaseConfig databaseConfig)
        {
            config = databaseConfig;
        }
    }
}