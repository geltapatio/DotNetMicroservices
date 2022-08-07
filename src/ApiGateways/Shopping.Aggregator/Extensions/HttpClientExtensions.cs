using System.Text.Json;

namespace Shopping.Aggregator.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://stackoverflow.com/questions/40027299/where-is-the-postasjsonasync-method-in-asp-net-core </remarks>
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
