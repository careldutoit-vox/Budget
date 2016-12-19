namespace EntityModels
{
    using Microsoft.Azure.Documents;
    using Newtonsoft.Json;

    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}