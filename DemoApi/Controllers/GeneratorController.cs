using System.Collections.Generic;
using Faker;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("[action]")]
    public class NamesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Names(Range range)
        {
            var names = range.Of(Name.FullName);
            return names;
        }

        [HttpGet]
        public IEnumerable<string> PhoneNumbers(Range range)
            => range.Of(Phone.Number);

    }
}
