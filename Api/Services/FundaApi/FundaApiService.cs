using Api.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace Api.Services.FundaApi
{
    public class FundaApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _key;

        public FundaApiService(IConfiguration configuration, HttpClient httpClient)
        {
            _key = configuration["FundaApiKey"] ?? throw new ConfigurationException("FundaApiKey is not defined in appsettings.");
            var apiUrl = configuration["FundaApiUrl"] ?? throw new ConfigurationException("FundaApiUrl is not defined in appsettings.");
            _httpClient = httpClient;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(apiUrl);
            }
        }

        public async Task<ApiResponse> GetHousesOnSaleAsync(List<string> filters, int page)
        {
            try
            {
                var url = $"/feeds/Aanbod.svc/json/{_key}/?type=koop&zo=/{GetFilters(filters)}/&page={page}&pagesize=25";
                var httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.StatusCode == HttpStatusCode.TooManyRequests || httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new LimitExceededException("API request limit exceeded.");
                }

                httpResponse.EnsureSuccessStatusCode(); 

                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var apiData = JsonSerializer.Deserialize<ApiResponse>(responseContent);
                return apiData;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while fetching data.", ex);
            }        
        }

        private string GetFilters(List<string> filters)
        {
            return string.Join("/", filters);
        }
    }
}
