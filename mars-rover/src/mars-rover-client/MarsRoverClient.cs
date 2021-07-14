using mars_rover_client.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace mars_rover_client
{
    public class MarsRoverClient : IMarsRoverClient
    {
        private string ApiKey => _configuration["NasaApiKey"];

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MarsRoverClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Response<IEnumerable<Rover>>> GetRoversAsync()
        {
            if (RateLimitWatcher.IsBlocked)
            {
                return null;
            }

            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"api/v1/rovers?api_key={ApiKey}"));

            RateLimitWatcher.SetFromResponseAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return new Response<IEnumerable<Rover>>
            {
                HttpStatusCode = (int)response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                Data = response.IsSuccessStatusCode
                    ? JsonConvert.DeserializeObject<RoversResponse>(responseJson).Rovers
                    : null
            };
        }

        public async Task<Response<IEnumerable<Photo>>> GetPhotosByEarthDateAsync(string rover, DateTime earthDate)
        {
            if (RateLimitWatcher.IsBlocked)
            {
                return null;
            }

            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"api/v1/rovers/{rover}/photos?earth_date={earthDate:yyyy-MM-dd}&api_key={ApiKey}"));

            RateLimitWatcher.SetFromResponseAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return new Response<IEnumerable<Photo>>
            {
                HttpStatusCode = (int)response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                Data = response.IsSuccessStatusCode
                    ? JsonConvert.DeserializeObject<RoverPhotosResponse>(responseJson).Photos
                    : null
            };
        }
    }
}
