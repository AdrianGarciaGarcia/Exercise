using Newtonsoft.Json;
using System.Text;
using UI.Infrastructure.Models;

namespace UI.Infrastructure
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<TopMakelaarsResponseModel> GetTopMakelaarsAsync(List<string> filters, int total)
        {
            var queryStringBuilder = new StringBuilder("/api/makelaars/top?");
            foreach (var filter in filters)
            {
                queryStringBuilder.Append($"filters={filter}&");
            }
            queryStringBuilder.Append($"total={total}");
            var url = queryStringBuilder.ToString();
            
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<TopMakelaarsResponseModel>(response);
                return data;
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching data.", ex);
            }
        }
    }
}
