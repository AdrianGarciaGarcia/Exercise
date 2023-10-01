using Api.Services.FundaApi;

namespace Api.Services
{
    public interface IApiService
    {
        public Task<ApiResponse> GetHousesOnSaleAsync(List<string> filters, int page);
    }
}
