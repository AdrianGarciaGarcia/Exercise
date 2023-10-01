using Api.Services.FundaApi;
using Api.Services;
using Api.Infrastructure.Exceptions;

namespace Api.Infrastructure.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddConfiguredApiService(this IServiceCollection services, IConfiguration configuration)
        {
            var apiBaseUrl = configuration["FundaApiUrl"];
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                throw new ConfigurationException("FundaApiUrl is not defined in appsettings.");
            }

            services.AddScoped<IApiService, FundaApiService>();
            services.AddHttpClient<FundaApiService>(c =>
            {
                c.BaseAddress = new Uri(apiBaseUrl);
            });

            return services;
        }
    }
}
