using Newtonsoft.Json;

namespace A1.SAS.Api.Query
{
    public class Dataquery
    {
        [JsonProperty(PropertyName = "filter")]
        public string? Filter { get; set; }
    }
}
