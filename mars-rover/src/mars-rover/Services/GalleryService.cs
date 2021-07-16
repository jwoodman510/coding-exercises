using AutoMapper;
using mars_rover.Models;
using mars_rover.Util;
using System.Collections.Generic;
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
        private readonly IDateFileParser _dateFileParser;

        public GalleryService(
            IMapper mapper,
            IRoverService roverService,
            IDateFileParser dateFileParser)
        {
            _mapper = mapper;
            _roverService = roverService;
            _dateFileParser = dateFileParser;
        }

        public async Task<IEnumerable<RoverPhotosModel>> GetAsync()
        {
            var dates = await _dateFileParser.GetDatesAsync("dates.txt");
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
    }
}
