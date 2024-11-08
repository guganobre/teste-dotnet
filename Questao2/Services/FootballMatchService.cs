using Newtonsoft.Json;
using Questao2.Model;
using Questao2.Util;

namespace Questao2.Services
{
    /// <summary>
    /// Serviço de conexão a api do Football Matches
    /// </summary>
    public class FootballMatchService
    {
        /// <summary>
        /// url api football match
        /// </summary>
        private const string urlAPI = "https://jsonmock.hackerrank.com/api/football_matches";

        /// <summary>
        /// Retorna a quandidades de gols marcados por um time em um ano
        /// </summary>
        /// <param name="team">Nome do time para consulta</param>
        /// <param name="year">Ano para consulta</param>
        /// <returns>Quantidade de gols marcados.</returns>
        public async Task<int> GetTotalScoredGoals(string team, int year)
        {
            // montando os paramêtros de acordo com o ano
            var paramTeam1 = new KeyValuePair<string, object>("team1", team);
            var paramTeam2 = new KeyValuePair<string, object>("team2", team);

            var paramYear = new KeyValuePair<string, object>("year", year);

            var matchsTeam1 = await getMatchAllAsync(paramYear, paramTeam1);
            var matchsTeam2 = await getMatchAllAsync(paramYear, paramTeam2);

            // buscando os gols de quando o time foi mandante e visitante dos jogos
            var totalGolsTeam1 = matchsTeam1.Sum(o => o.Team1Goals);
            var totalGolsTeam2 = matchsTeam2.Sum(o => o.Team2Goals);

            return totalGolsTeam1 + totalGolsTeam2;
        }

        /// <summary>
        /// Retorna o response da API do football matches.
        /// </summary>
        /// <param name="param">Filtro</param>
        /// <returns>Response API football matches</returns>
        private async Task<ResponseFootballMatches?> getAsync(params KeyValuePair<string, object>[] param)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // montagem dos parametros para envio via get
                string queryString = "";
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        queryString = queryString.AddParamToQuery(item.Key, item.Value);
                    }
                }

                string url = $"{urlAPI}{queryString}";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseFootballMatches>(content);

                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Retorna todas as partidas.
        /// </summary>
        /// <param name="param">filtro das partidas.</param>
        /// <returns>Todas as partidas de acordo com o filtro.</returns>
        private async Task<List<Match>> getMatchAllAsync(params KeyValuePair<string, object>[] param)
        {
            var result = new List<Match>();

            int page = 1;
            bool finishPage = false;

            while (!finishPage)
            {
                var newParam = param.ToList();
                newParam.Add(new KeyValuePair<string, object>("page", page));

                var resultMatchs = await getAsync(newParam.ToArray());

                result.AddRange(resultMatchs.Data);

                finishPage = resultMatchs.Page == resultMatchs.TotalPages;
                page = resultMatchs.Page + 1;
            }

            return result;
        }
    }
}