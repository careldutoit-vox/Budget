
namespace EntityModels
{
    using Microsoft.Azure.Documents;
    using Newtonsoft.Json;
    public class Values_Audits : EntityBase
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }
    }
}
