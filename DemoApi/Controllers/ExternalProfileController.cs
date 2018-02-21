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
        private readonly IBusControl _bus;

        public ExternalProfileController(ProfileService profileService, IBusControl bus)
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
                    var addUserEndpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://rabbit/dockerdemo/SendUserProfile"));
                    await addUserEndpoint.Send<ISendUserProfile>(new {UserProfile = profile});
                    Console.WriteLine("=====> Message sent over RabbitMq/MassTransit.");
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
