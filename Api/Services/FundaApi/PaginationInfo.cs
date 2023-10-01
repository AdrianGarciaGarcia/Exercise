using System.Text.Json.Serialization;

namespace Api.Services.FundaApi
{
    public class PaginationInfo
    {
        [JsonPropertyName("AantalPaginas")]
        public int TotalPages { get; set; }
    }
}
