using System;
using System.Threading.Tasks;
using DemoApi.Services;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    public class ExternalProfileController : Controller
    {
        private readonly ProfileService _profileService;
        private readonly IBus _bus;

        public ExternalProfileController(ProfileService profileService, IBus bus)
        {
            _profileService = profileService;
            _bus = bus;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var profile = await _profileService.GetExternalProfile(id);

                if (profile != null)
                {
                    var addUserEndpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://rabbit/SendUserProfile"));
                    await addUserEndpoint.Send<ISendUserProfile>(new {UserProfile = profile});
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
