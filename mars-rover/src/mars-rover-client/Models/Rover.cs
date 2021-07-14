using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace mars_rover_client.Models
{
    public partial class Rover
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("landing_date")]
        public DateTime LandingDate { get; set; }

        [JsonProperty("launch_date")]
        public DateTime LaunchDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("max_sol")]
        public long MaxSol { get; set; }

        [JsonProperty("max_date")]
        public DateTime MaxDate { get; set; }

        [JsonProperty("total_photos")]
        public long TotalPhotos { get; set; }

        [JsonProperty("cameras")]
        public List<Camera> Cameras { get; set; }
    }
}
