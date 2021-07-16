using AutoMapper;
using mars_rover.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace mars_rover.Services
{
    public interface IGalleryService
    {
        Task<IEnumerable<RoverPhotosModel>> GetAsync();

        Task<Stream> GetStreamAsync();
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

        public async Task<Stream> GetStreamAsync()
        {
            var gallery = await GetAsync();

            var photos = gallery.SelectMany(x => x.Photos);

            var getImageTasks = photos.Select(async photo =>
            {
                using var webClient = new WebClient();

                var bytes = await webClient.DownloadDataTaskAsync(photo.ImgSrc);

                return new
                {
                    Name = photo.ImgSrc.ToString().Split('/').Last(),
                    Data = bytes
                };
            });

            var memoryStream = new MemoryStream();

            using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);

            foreach (var imageResult in await Task.WhenAll(getImageTasks))
            {
                var zipEntry = archive.CreateEntry(imageResult.Name, CompressionLevel.Fastest);

                using var zipStream = zipEntry.Open();

                await zipStream.WriteAsync(imageResult.Data);
            }

            return memoryStream;
        }
    }
}
