using Newtonsoft.Json;

namespace mars_rover_client.Models
{
    public class RoversResponse
    {
        [JsonProperty("rovers")]
        public Rover[] Rovers { get; set; }
    }
}
