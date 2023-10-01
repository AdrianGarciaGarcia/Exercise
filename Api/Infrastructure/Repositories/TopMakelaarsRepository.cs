using Api.Application.Interfaces;
using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Models;
using Api.Services;
using Api.Services.FundaApi;
using Microsoft.Extensions.Caching.Memory;
using static Api.Application.UseCases.GetTopMakelaars;

namespace Api.Infrastructure.Repositories
{
    public class TopMakelaarsRepository : ITopMakelaarsRepository
    {
        private readonly IApiService _apiService;
        private readonly IMemoryCache _cache;
        public TopMakelaarsRepository(IApiService apiService, IMemoryCache cache)
        {
            _apiService = apiService;
            _cache = cache;
        }

        public async Task<GetTopMakelaarsResponse> GetTopMakelaarsAsync(List<string> filters, int total)
        {
            var allMakelaars = new Dictionary<int, Makelaar>();
            var page = 1;
            var incompleteData = false;

            while (true)
            {
                try {

                    var cacheKey = $"HousesOnSale-{string.Join("-", filters)}-{page}";
                    if (!_cache.TryGetValue(cacheKey, out ApiResponse? housesInfo))
                    {
                        housesInfo = await _apiService.GetHousesOnSaleAsync(filters, page: page);
                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
                        };

                        _cache.Set(cacheKey, housesInfo, cacheEntryOptions);
                    }

                    if (housesInfo == null)
                        break;

                    foreach (var house in housesInfo.Houses)
                    {
                        if (!allMakelaars.TryGetValue(house.MakelaarId, out var makelaar))
                        {
                            makelaar = new Makelaar() { Id = house.MakelaarId, Name = house.MakelaarName, TotalHouses = 1 };
                            allMakelaars[key: house.MakelaarId] = makelaar;
                        }
                        else
                            makelaar.TotalHouses++;
                    }

                    if (page >= housesInfo.Pagination.TotalPages)
                        break;

                    page++;
                } 
                catch(Exception ex)
                {
                    if (ex is LimitExceededException)
                    {
                        incompleteData = true;
                        break;
                    }
                    else 
                        throw new Exception("An error occurred while fetching data.", ex);
                }                
            }

            var topMakelaars = allMakelaars.Values
                .OrderByDescending(m => m.TotalHouses)
                .Take(total)
                .Select((makelaar, index) => new TopMakelaar(index + 1, makelaar.Name, makelaar.TotalHouses))
                .ToList();

            return new GetTopMakelaarsResponse(topMakelaars, incompleteData);
        }
    }
}
