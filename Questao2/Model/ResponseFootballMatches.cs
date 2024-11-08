using Newtonsoft.Json;

namespace Questao2.Model
{
    public class ResponseFootballMatches
    {
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        public List<Match> Data { get; set; }
    }
}