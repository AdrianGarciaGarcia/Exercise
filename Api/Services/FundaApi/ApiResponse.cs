using System.Text.Json.Serialization;

namespace Api.Services.FundaApi
{
    public class ApiResponse
    {
        [JsonPropertyName("Objects")]
        public List<HouseInfo> Houses { get; set; }

        [JsonPropertyName("Paging")]
        public PaginationInfo Pagination { get; set; }
    }
}
