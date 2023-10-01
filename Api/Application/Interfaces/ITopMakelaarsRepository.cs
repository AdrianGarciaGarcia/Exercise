using static Api.Application.UseCases.GetTopMakelaars;

namespace Api.Application.Interfaces
{
    public interface ITopMakelaarsRepository
    {
        public Task<GetTopMakelaarsResponse> GetTopMakelaarsAsync(List<string> filters, int Total);
    }
}
