using System;
using Newtonsoft.Json;

namespace Apprenticeship.Function.CosmosDB
{
    [Serializable]
    public class GeoPoint
    {
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "easting")]
        public int Easting { get; set; }

        [JsonProperty(PropertyName = "northing")]
        public int Northing { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}