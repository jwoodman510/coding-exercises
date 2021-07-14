using mars_rover_client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mars_rover_client
{
    public interface IMarsRoverClient
    {
        Task<Response<IEnumerable<Rover>>> GetRoversAsync();

        Task<Response<IEnumerable<Photo>>> GetPhotosByEarthDateAsync(string rover, DateTime earthDate);
    }
}
