using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Apprenticeship.Function.CosmosDB
{
    [Serializable]
    public class CosmosDbVacancySummary
    {
        [JsonProperty(PropertyName = "vacancysummaries")]
        public IEnumerable<VacancySummary> VacancySummaries { get; set; }

        [JsonProperty(PropertyName = "totalcount")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "currentpage")]
        public int CurrentPage { get; set; }

        [JsonProperty(PropertyName = "totalpages")]
        public int TotalPages { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
