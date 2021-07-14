using Newtonsoft.Json;
using System;

namespace mars_rover_client.Models
{
    public partial class Photo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("sol")]
        public long Sol { get; set; }

        [JsonProperty("camera")]
        public Camera Camera { get; set; }

        [JsonProperty("img_src")]
        public Uri ImgSrc { get; set; }

        [JsonProperty("earth_date")]
        public DateTime EarthDate { get; set; }

        [JsonProperty("rover")]
        public Rover Rover { get; set; }
    }
}
