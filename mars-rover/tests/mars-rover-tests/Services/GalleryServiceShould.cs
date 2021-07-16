using mars_rover.Models;
using mars_rover.Services;
using mars_rover_tests._Shared;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace mars_rover_tests
{
    public class GalleryServiceShould
    {
        private readonly DateTime[] Dates = new DateTime[] { DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(-2) };
        private readonly RoverModel[] Rovers = new RoverModel[] { new RoverModel { Name = "A" }, new RoverModel { Name = "B" } };

        private readonly Mock<IRoverService> _roverServiceMock;
        private readonly Mock<IDateFileParser> _dateFileParserMock;

        private readonly GalleryService _service;

        public GalleryServiceShould()
        {
            _roverServiceMock = new Mock<IRoverService>();
            _dateFileParserMock = new Mock<IDateFileParser>();

            _dateFileParserMock
                .Setup(x => x.GetDatesAsync(It.IsAny<string>()))
                .ReturnsAsync(Dates);

            _roverServiceMock
                .Setup(x => x.GetRoversAsync())
                .ReturnsAsync(Rovers);

            _roverServiceMock
                .Setup(x => x.GetPhotosAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(Enumerable.Empty<PhotoModel>());

            _service = new GalleryService(
                MockUtil.Mapper,
                _roverServiceMock.Object,
                _dateFileParserMock.Object);
        }

        [Fact(DisplayName = "Retrieve Rovers")]
        public async Task RetrieveMultipleRovers()
        {
            var gallery = await _service.GetAsync();

            Assert.NotNull(gallery);
            Assert.NotEmpty(gallery);
            Assert.Contains(gallery, x => x.Name == "A");
            Assert.Contains(gallery, x => x.Name == "B");
        }

        [Fact(DisplayName = "Retrieve Rover Photos By Date")]
        public async Task RetrieveRoverPhotosByDate()
        {
            _roverServiceMock
                .Setup(x => x.GetPhotosAsync("A", Dates[0]))
                .ReturnsAsync(new PhotoModel[] { new PhotoModel { EarthDate = Dates[0], ImgSrc = new Uri("http://localhost.com/1") } });

            _roverServiceMock
                .Setup(x => x.GetPhotosAsync("A", Dates[1]))
                .ReturnsAsync(new PhotoModel[] { new PhotoModel { EarthDate = Dates[1], ImgSrc = new Uri("http://localhost.com/2") } });

            var gallery = await _service.GetAsync();

            var rover = gallery.Single(x => x.Name == "A");

            Assert.NotNull(rover.Photos);
            Assert.NotEmpty(rover.Photos);
            Assert.Contains(rover.Photos, x => x.EarthDate.ToString("yyyy-MM-DD") == Dates[0].ToString("yyyy-MM-DD") && x.ImgSrc == new Uri("http://localhost.com/1"));
            Assert.Contains(rover.Photos, x => x.EarthDate.ToString("yyyy-MM-DD") == Dates[0].ToString("yyyy-MM-DD") && x.ImgSrc == new Uri("http://localhost.com/2"));
        }
    }
}
