using Api.Services.FundaApi;
using Api.Services;
using Api.Infrastructure.Exceptions;

namespace Api.Infrastructure.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddConfiguredApiService(this IServiceCollection services, IConfiguration configuration)
        {
            var apiBaseUrl = configuration["FundaApi:Url"];
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                throw new ConfigurationException("FundaApiUrl is not defined in appsettings.");
            }

            services.AddSingleton<IApiService, FundaApiService>();
            services.AddHttpClient<FundaApiService>();

            return services;
        }
    }
}
