using AutoMapper;
using mars_rover.Models;

namespace mars_rover_tests._Shared
{
    public static class MockUtil
    {
        public static IMapper Mapper => new MapperConfiguration(x => x.AddProfile<AutoMapperProfile>()).CreateMapper();
    }
}
