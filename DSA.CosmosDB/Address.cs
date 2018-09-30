using System;
using Newtonsoft.Json;

namespace Apprenticeship.Function.CosmosDB
{
    [Serializable]
    public class Address
    {
        [JsonProperty(PropertyName = "postaladdressid")]
        public int PostalAddressId { get; set; }

        [JsonProperty(PropertyName = "addressline1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressline2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "addressline3")]
        public string AddressLine3 { get; set; }

        [JsonProperty(PropertyName = "addressline4")]
        public string AddressLine4 { get; set; }

        [JsonProperty(PropertyName = "town")]
        public string Town { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }

        [JsonProperty(PropertyName = "datevalidated")]
        public string DateValidated { get; set; }

        [JsonProperty(PropertyName = "countyid")]
        public int CountyId { get; set; }

        [JsonProperty(PropertyName = "localauthroityid")]
        public int LocalAuthorityId { get; set; }

        [JsonProperty(PropertyName = "geopoint")]
        public GeoPoint GeoPoint { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}