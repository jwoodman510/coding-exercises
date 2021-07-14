using mars_rover.Models;
using mars_rover.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mars_rover.Controllers
{
    public class RoverController : BaseController
    {
        private readonly IRoverService _roverService;

        public RoverController(IRoverService roverService)
        {
            _roverService = roverService;
        }

        [HttpGet]
        public Task<IEnumerable<RoverModel>> Get() => _roverService.GetRoversAsync();

        [HttpGet("Photos")]
        public Task<IEnumerable<PhotoModel>> GetPhotos([FromQuery] string rover, [FromQuery] DateTime date) => _roverService.GetPhotosAsync(rover, date);
    }
}
