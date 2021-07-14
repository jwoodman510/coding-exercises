using mars_rover.Models;
using mars_rover.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mars_rover.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet]
        public Task<IEnumerable<RoverPhotosModel>> Get() => _galleryService.GetAsync();
    }
}
