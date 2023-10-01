using System.Text.Json.Serialization;

namespace Api.Services.FundaApi
{
    public class HouseInfo
    {
        [JsonPropertyName("MakelaarId")]
        public int MakelaarId { get; set; }

        [JsonPropertyName("MakelaarNaam")]
        public string MakelaarName { get; set; }
    }
}
