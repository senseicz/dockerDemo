using System;
using System.Threading.Tasks;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    public class ExternalProfileController : Controller
    {
        private ProfileService _profileService;

        public ExternalProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var profile = await _profileService.GetExternalProfile(id);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
