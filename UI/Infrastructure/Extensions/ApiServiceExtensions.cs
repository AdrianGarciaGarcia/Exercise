using UI.Infrastructure.Exceptions;

namespace UI.Infrastructure.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddConfiguredApiService(this IServiceCollection services, IConfiguration configuration)
        {
            var apiBaseUrl = configuration["InternalApiUrl"];
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                throw new ConfigurationException("InternalApiUrl is not defined in appsettings.");
            }

            services.AddScoped<ApiService>();
            services.AddHttpClient<ApiService>(c =>
            {
                c.BaseAddress = new Uri(apiBaseUrl);
            });

            return services;
        }
    }
}
