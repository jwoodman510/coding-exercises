using Newtonsoft.Json;

namespace mars_rover_client.Models
{
    public class Camera
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rover_id")]
        public long RoverId { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }
}
