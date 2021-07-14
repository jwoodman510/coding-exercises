using AutoMapper;
using mars_rover.Models;
using mars_rover_client;
using mars_rover_client.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mars_rover.Services
{
    public interface IRoverService
    {
        Task<IEnumerable<RoverModel>> GetRoversAsync();

        Task<IEnumerable<PhotoModel>> GetPhotosAsync(string rover, DateTime date);
    }

    public class RoverService : IRoverService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IMarsRoverClient _marsRoverClient;

        public RoverService(
            IMapper mapper,
            IMemoryCache memoryCache,
            IMarsRoverClient marsRoverClient)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _marsRoverClient = marsRoverClient;
        }

        public async Task<IEnumerable<RoverModel>> GetRoversAsync()
        {
            return await _memoryCache.GetOrCreateAsync($"", async entry =>
            {
                var response = await _marsRoverClient.GetRoversAsync();

                entry.AbsoluteExpirationRelativeToNow = response.IsSuccessStatusCode
                    ? TimeSpan.FromHours(1)
                    : TimeSpan.FromSeconds(-1);

                var rovers = response.Data ?? Enumerable.Empty<Rover>();

                return _mapper.Map<IEnumerable<RoverModel>>(rovers);
            });
        }

        public async Task<IEnumerable<PhotoModel>> GetPhotosAsync(string rover, DateTime date)
        {
            if (string.IsNullOrEmpty(rover) || date == DateTime.MinValue)
            {
                return Enumerable.Empty<PhotoModel>();
            }

            return await _memoryCache.GetOrCreateAsync($"{nameof(RoverService.GetPhotosAsync)}:{rover}:{date:yyyy-MM-dd}", async entry =>
            {
                var response = await _marsRoverClient.GetPhotosByEarthDateAsync(rover, date);

                entry.AbsoluteExpirationRelativeToNow = response.IsSuccessStatusCode
                    ? TimeSpan.FromHours(1)
                    : TimeSpan.FromSeconds(-1);

                var photos = response.Data ?? Enumerable.Empty<Photo>();

                return _mapper.Map<IEnumerable<PhotoModel>>(photos);
            });
        }
    }
}
