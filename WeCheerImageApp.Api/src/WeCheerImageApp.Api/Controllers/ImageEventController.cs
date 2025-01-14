using Microsoft.AspNetCore.Mvc;
using WeCheerImageApp.Api.Models;
using WeCheerImageApp.Api.Services;

namespace WeCheerImageApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageEventController : ControllerBase
    {
        private readonly IImageEventService _imageEventService;

        public ImageEventController(IImageEventService imageEventService)
        {
            _imageEventService = imageEventService;
        }

        [HttpPost]
        public IActionResult AddEvent([FromBody] ImageEvent imageEvent)
        {
            _imageEventService.AddEvent(imageEvent);
            return Ok();
        }

        [HttpGet("latest")]
        public IActionResult GetLatestEvent()
        {
            var latestEvent = _imageEventService.GetLatestEvent();
            if (latestEvent == null)
                return NotFound();
            return Ok(latestEvent);
        }

        [HttpGet("count")]
        public IActionResult GetEventCountLastHour()
        {
            var count = _imageEventService.GetEventCountLastHour();
            return Ok(new { Count = count });
        }
    }
} 