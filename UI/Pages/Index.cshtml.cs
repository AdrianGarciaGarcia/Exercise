using Microsoft.AspNetCore.Mvc.RazorPages;
using UI.Infrastructure;
using UI.Infrastructure.Models;

namespace Exercise.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;
        public TopMakelaarsResponseModel TableTopMakelaars { get; private set; }
        public TopMakelaarsResponseModel TableTopMakelaarsHousesWithGarden { get; private set; }

        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task OnGetAsync()
        {
            TableTopMakelaars = await _apiService.GetTopMakelaarsAsync(new List<string>() { "amsterdam" }, 10);
            TableTopMakelaarsHousesWithGarden = await _apiService.GetTopMakelaarsAsync(new List<string>() { "amsterdam", "tuin" }, 10);
        }
    }
}