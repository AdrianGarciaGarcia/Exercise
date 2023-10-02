using Api.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace Api.Services.FundaApi
{
    public class FundaApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _key; 
        private readonly int _maxCallsPerMinute; 
        private readonly List<DateTime> _lastCallsTimestamps = new();

        public FundaApiService(IConfiguration configuration, HttpClient httpClient)
        {
            _key = configuration["FundaApi:Key"] ?? throw new ConfigurationException("FundaApiKey is not defined in appsettings.");
            _maxCallsPerMinute = int.TryParse(configuration["FundaApi:MaxCallsPerMinute"], out int result) ? result : 100;
            _httpClient = httpClient;
            var apiUrl = configuration["FundaApi:Url"] ?? throw new ConfigurationException("FundaApiUrl is not defined in appsettings.");

            if (_httpClient.BaseAddress == null)
                _httpClient.BaseAddress = new Uri(apiUrl);
        }

        public async Task<ApiResponse> GetHousesOnSaleAsync(List<string> filters, int page)
        {
            EnsureCallsPerMinuteLimit();

            try
            {
                var url = $"/feeds/Aanbod.svc/json/{_key}/?type=koop&zo=/{GetFilters(filters)}/&page={page}&pagesize=25";
                var httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.StatusCode == HttpStatusCode.TooManyRequests || httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                    throw new LimitExceededException("API request limit exceeded.");

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

        private void EnsureCallsPerMinuteLimit()
        {
            _lastCallsTimestamps.RemoveAll(timestamp => DateTime.UtcNow - timestamp > TimeSpan.FromMinutes(1));

            if (_lastCallsTimestamps.Count >= _maxCallsPerMinute)
                throw new LimitExceededException("API request limit exceeded.");

            _lastCallsTimestamps.Add(DateTime.UtcNow);
        }

        private string GetFilters(List<string> filters)
        {
            return string.Join("/", filters);
        }
    }
}
