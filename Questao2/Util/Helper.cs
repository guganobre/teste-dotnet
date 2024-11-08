namespace Questao2.Util
{
    /// <summary>
    /// Classe base para auxíliar conversão de valores
    /// </summary>
    public static class Helper
    {
        public static string AddParamToQuery(this string queryString, string key, object value)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                queryString = "?";
            }
            else
            {
                queryString += "&";
            }

            queryString += $"{key}={value}";

            return queryString;
        }
    }
}