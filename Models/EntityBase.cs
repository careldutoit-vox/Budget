﻿namespace Models
{
    using Microsoft.Azure.Documents;
    using Newtonsoft.Json;

    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}