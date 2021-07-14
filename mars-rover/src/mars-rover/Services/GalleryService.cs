using AutoMapper;
using mars_rover.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mars_rover.Services
{
    public interface IGalleryService
    {
        Task<IEnumerable<RoverPhotosModel>> GetAsync();
    }

    public class GalleryService : IGalleryService
    {
        private readonly IMapper _mapper;
        private readonly IRoverService _roverService;

        public GalleryService(
            IMapper mapper,
            IRoverService roverService)
        {
            _mapper = mapper;
            _roverService = roverService;
        }

        public async Task<IEnumerable<RoverPhotosModel>> GetAsync()
        {
            var dates = await GetDatesAsync();
            var rovers = await _roverService.GetRoversAsync();

            return await Task.WhenAll(rovers.Select(async rover =>
            {
                var roverPhotos = _mapper.Map<RoverPhotosModel>(rover);

                var getPhotos = dates.Select(date => _roverService.GetPhotosAsync(rover.Name, date));

                await Task.WhenAll(getPhotos);

                roverPhotos.Photos = getPhotos.SelectMany(x => x.Result).ToList();

                return roverPhotos;
            }));
        }

        private static async Task<IEnumerable<DateTime>> GetDatesAsync()
        {
            var dates = new List<DateTime>();

            if (!File.Exists("dates.txt"))
            {
                return dates;
            }

            using var filestream = File.OpenRead("dates.txt");
            using var streamReader = new StreamReader(filestream);

            var line = await streamReader.ReadLineAsync();

            while (line != null)
            {
                if (DateTime.TryParse(line, out DateTime dateTime))
                {
                    dates.Add(dateTime);
                }

                line = await streamReader.ReadLineAsync();
            }

            return dates.Distinct();
        }
    }
}
