using AutoMapper;
using mars_rover_client.Models;

namespace mars_rover.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Rover, RoverModel>();
            CreateMap<Photo, PhotoModel>();
            CreateMap<RoverModel, RoverPhotosModel>();
        }
    }
}
